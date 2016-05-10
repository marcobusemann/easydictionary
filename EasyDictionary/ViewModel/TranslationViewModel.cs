using EasyDictionary.Domain;
using EasyDictionary.Exceptions;
using EasyDictionary.Services;
using EasyDictionary.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Storage;

namespace EasyDictionary.ViewModel
{
    public static class NotifyPropertyChangedExtensions
    {
        // See: http://blog.pieeatingninjas.be/2015/11/14/using-rx-in-a-uwp-project-with-mvvm/
        public static IObservable<T> GetPropertyAsObservable<T>(this INotifyPropertyChanged @class, Expression<Func<T>> property)
        {
            var propName = (property.Body as MemberExpression).Member.Name;
            return Observable.FromEventPattern<PropertyChangedEventArgs>(@class, nameof(@class.PropertyChanged))
                .Where(a => a.EventArgs.PropertyName == propName)
                .Select(a => property.Compile().Invoke());
        }
    }

    public class TranslationViewModel: INotifyPropertyChanged
    {
        const String SETTING_LASTLANGUAGEOPTION = "lastlanguageoption";
        const String SETTING_LASTLANGUAGEOPTION_DEST = "dest";
        const String SETTING_LASTLANGUAGEOPTION_SRC = "src";

        #region events
        public EventHandler<String> SearchStringChanged;
        public EventHandler<TranslationLanguageOption> TranslationOptionChanged;
        #endregion

        #region Properties
        private ITranslationService _translationService;

        private string _searchString = String.Empty;
        public string SearchString
        {
            get { return _searchString; }
            set
            {
                if (_searchString == value) return;
                _searchString = value;
                Exception = null;
                NotifyPropertyChanged();
                if (SearchStringChanged != null)
                    SearchStringChanged(this, _searchString);
            }
        }

        private IEnumerable<Translation> _translations;
        public IEnumerable<Translation> Translations
        {
            get { return _translations; }
            set
            {
                if (_translations == value) return;
                _translations = value;
                NotifyPropertyChanged();
            }
        }

        private TranslationLanguageOption _selectedTranslationOption;
        public TranslationLanguageOption SelectedTranslationOption
        {
            get { return _selectedTranslationOption; }
            set
            {
                if (_selectedTranslationOption == value) return;
                _selectedTranslationOption = value;
                Exception = null;
                NotifyPropertyChanged();
                SaveLastTranslationOption(_selectedTranslationOption);
                if (TranslationOptionChanged != null)
                    TranslationOptionChanged(this, _selectedTranslationOption);
            }
        }

        private IEnumerable<TranslationLanguageOption> _translationOptions;
        public IEnumerable<TranslationLanguageOption> TranslationOptions
        {
            get { return _translationOptions; }
            set
            {
                if (_translationOptions == value) return;
                _translationOptions = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("HasTranslationOptions");
            }
        }

        private TranslatableException _exception = null;
        private TranslatableException Exception
        {
            get { return _exception; }
            set
            {
                _exception = value;
                NotifyPropertyChanged("TranslatedExceptionMessage");
                NotifyPropertyChanged("HasError");
            }
        }

        public string TranslatedExceptionMessage
        {
            get
            {
                return HasError ? Exception.Translate(new ResourceLoader()) : string.Empty;
            }
        }

        public bool HasError
        {
            get
            {
                return _exception != null;
            }
        }
        #endregion

        public TranslationViewModel(ITranslationService translationService)
        {
            _translationService = translationService;

            TranslationOptionChanged += async (o, e) => await Translate();

            SetupDelayedSearch();
        }

        private void SetupDelayedSearch()
        {
            // See: http://blog.pieeatingninjas.be/2015/11/14/using-rx-in-a-uwp-project-with-mvvm/
            var throttledTextChangeSequence = this.GetPropertyAsObservable(() => SearchString)
                .DistinctUntilChanged()
                .Throttle(TimeSpan.FromSeconds(0.5));

            var result = from qry in throttledTextChangeSequence
                         from translations in _translationService.LoadTranslations(SelectedTranslationOption, qry)
                         select translations;

            throttledTextChangeSequence
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(e => Translations = null);

            result.ObserveOn(SynchronizationContext.Current)
                .Subscribe(
                    t => Translations = t, 
                    exception => {
                        Exception = exception is TranslatableException ? 
                            exception as TranslatableException : 
                            new UnknownErrorException();
                        // TODO: Find out how to use SubscribeSave
                        SetupDelayedSearch();
                });
        }

        public async Task Initialize()
        {
            await EmbeddExceptionHandlingAsync(async () =>
            {
                TranslationOptions = await _translationService.LoadTranslationOptions();
                SelectedTranslationOption = LoadLastTranslationOption(TranslationOptions) ?? TranslationOptions.First();
            });
        }

        private TranslationLanguageOption LoadLastTranslationOption(IEnumerable<TranslationLanguageOption> options)
        {
            TranslationLanguageOption result = null;
            ApplicationDataContainer AppSettings = ApplicationData.Current.RoamingSettings;
            var values = AppSettings.Values;

            if (values.ContainsKey(SETTING_LASTLANGUAGEOPTION))
            {
                var value = values[SETTING_LASTLANGUAGEOPTION] as ApplicationDataCompositeValue;

                result = options.Where(o =>
                    o.Destination.LanguageTag == value[SETTING_LASTLANGUAGEOPTION_DEST] as String &&
                    o.Source.LanguageTag == value[SETTING_LASTLANGUAGEOPTION_SRC] as String).FirstOrDefault();
            }

            return result;
        }

        private void SaveLastTranslationOption(TranslationLanguageOption option)
        {
            ApplicationDataContainer AppSettings = ApplicationData.Current.RoamingSettings;

            var composite = new ApplicationDataCompositeValue();
            composite[SETTING_LASTLANGUAGEOPTION_SRC] = option.Source.LanguageTag;
            composite[SETTING_LASTLANGUAGEOPTION_DEST] = option.Destination.LanguageTag;

            AppSettings.Values[SETTING_LASTLANGUAGEOPTION] = composite;
        }

        private async Task Translate()
        {
            await EmbeddExceptionHandlingAsync(async () =>
            {
                Translations = await _translationService.LoadTranslations(SelectedTranslationOption, SearchString);
            });
        }

        private async Task EmbeddExceptionHandlingAsync(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (TranslatableException e)
            {
                Exception = e;
            }
            catch (Exception)
            {
                Exception = new UnknownErrorException();
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
