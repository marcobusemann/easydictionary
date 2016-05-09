using EasyDictionary.Domain;
using EasyDictionary.Services;
using EasyDictionary.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Vorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 dokumentiert.

namespace EasyDictionary.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var viewModel = new MainPageViewModel();
            DataContext = viewModel;
        }

        /*
        public MainPage()
        {
            this.InitializeComponent();
            SizeChanged += MainPage_SizeChanged;

            //SettingsPane pane = SettingsPane.GetForCurrentView();
            //pane.CommandsRequested += pane_CommandsRequested;
        }

        const String URL_IMPRESS = "http://www.blural.de/#/legal#impressum";
        const String URL_DATENSCHUTZ = "http://www.blural.de/#/legal#datenschutz";
        const String URL_HAFTUNGSAUSSCHLUSS = "http://www.blural.de/#/legal#haftungsausschluss";
        const String URL_FEEDBACK = "http://www.blural.de/#/requests";

        public static readonly DependencyProperty TranslationsProperty = DependencyProperty.Register("Translations", typeof(TranslationRequest), typeof(MainPage), new PropertyMetadata(null));
        public TranslationRequest Translations
        {
            get
            {
                return (TranslationRequest)GetValue(TranslationsProperty);
            }
            set
            {
                SetValue(TranslationsProperty, value);
            }
        }

        public static readonly DependencyProperty LanguageCombinationsProperty = DependencyProperty.Register("LanguageCombinations", typeof(List<TranslationLanguageOption>), typeof(MainPage), new PropertyMetadata(null));
        public List<TranslationLanguageOption> LanguageCombinations
        {
            get
            {
                return (List<TranslationLanguageOption>)GetValue(LanguageCombinationsProperty);
            }
            set
            {
                SetValue(LanguageCombinationsProperty, value);
            }
        }

        public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register("SearchText", typeof(String), typeof(MainPage), new PropertyMetadata(null));
        public String SearchText
        {
            get
            {
                return (String)GetValue(SearchTextProperty);
            }
            set
            {
                SetValue(SearchTextProperty, value);
            }
        }

        public static readonly DependencyProperty LanguageCombinationProperty = DependencyProperty.Register("LanguageCombination", typeof(TranslationLanguageOption), typeof(MainPage), new PropertyMetadata(null));
        public TranslationLanguageOption LanguageCombination
        {
            get
            {
                return (TranslationLanguageOption)GetValue(LanguageCombinationProperty);
            }
            set
            {
                SetValue(LanguageCombinationProperty, value);
            }
        }

        public static readonly DependencyProperty HasLanguagesProperty = DependencyProperty.Register("HasLanguages", typeof(Boolean), typeof(MainPage), new PropertyMetadata(false));
        public Boolean HasLanguages
        {
            get
            {
                return (Boolean)GetValue(HasLanguagesProperty);
            }
            set
            {
                SetValue(HasLanguagesProperty, value);
            }
        }

        public static readonly DependencyProperty HasAvailableLanguagesProperty = DependencyProperty.Register("HasAvailableLanguages", typeof(Boolean), typeof(MainPage), new PropertyMetadata(false));
        public Boolean HasAvailableLanguages
        {
            get
            {
                return (Boolean)GetValue(HasAvailableLanguagesProperty);
            }
            set
            {
                SetValue(HasAvailableLanguagesProperty, value);
            }
        }

        public static readonly DependencyProperty HasErrorProperty = DependencyProperty.Register("HasError", typeof(Boolean), typeof(MainPage), new PropertyMetadata(false));
        public Boolean HasError
        {
            get
            {
                return (Boolean)GetValue(HasErrorProperty);
            }
            set
            {
                SetValue(HasErrorProperty, value);
            }
        }

        public static readonly DependencyProperty ShowOwnAddProperty = DependencyProperty.Register("ShowOwnAdd", typeof(Boolean), typeof(MainPage), new PropertyMetadata(false));
        public Boolean ShowOwnAdd
        {
            get
            {
                return (Boolean)GetValue(ShowOwnAddProperty);
            }
            set
            {
                SetValue(ShowOwnAddProperty, value);
            }
        }

            void pane_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            ResourceLoader translations = new ResourceLoader();

            args.Request.ApplicationCommands.Add(new SettingsCommand(translations.GetString("SettingsInfo"), translations.GetString("SettingsInfo"), (handler) =>
            {
                var pane = new Dictionary.SettingInfo();
                pane.Show();
            }));

            args.Request.ApplicationCommands.Add(new SettingsCommand(translations.GetString("SettingsLegalNotice"), translations.GetString("SettingsLegalNotice"), async (handler) =>
            {
                var options = new LauncherOptions();
                options.TreatAsUntrusted = true;
                await Launcher.LaunchUriAsync(new Uri(URL_IMPRESS), options);
            }));

            args.Request.ApplicationCommands.Add(new SettingsCommand(translations.GetString("SettingsPrivacy"), translations.GetString("SettingsPrivacy"), async (handler) =>
            {
                var options = new LauncherOptions();
                options.TreatAsUntrusted = true;
                await Launcher.LaunchUriAsync(new Uri(URL_DATENSCHUTZ), options);
            }));

            args.Request.ApplicationCommands.Add(new SettingsCommand(translations.GetString("SettingsDisclaimer"), translations.GetString("SettingsDisclaimer"), async (handler) =>
            {
                var options = new LauncherOptions();
                options.TreatAsUntrusted = true;
                await Launcher.LaunchUriAsync(new Uri(URL_HAFTUNGSAUSSCHLUSS), options);
            }));

            args.Request.ApplicationCommands.Add(new SettingsCommand(translations.GetString("SettingsFeedback"), translations.GetString("SettingsFeedback"), async (handler) =>
            {
                var options = new LauncherOptions();
                options.TreatAsUntrusted = true;
                await Launcher.LaunchUriAsync(new Uri(URL_FEEDBACK), options);
            }));
        }
        void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ApplicationView view = ApplicationView.GetForCurrentView();
            if (e.NewSize.Width > 650)
            {
                VisualStateManager.GoToState(this, "DefaultLayout", true);
                if (DefaultViewSearchBox.FocusState == Windows.UI.Xaml.FocusState.Unfocused)
                    DefaultViewSearchBox.Focus(FocusState.Keyboard);
            }
            else
            {
                VisualStateManager.GoToState(this, "SmallLayout", true);
                if (SmallViewSearchBox.FocusState == Windows.UI.Xaml.FocusState.Unfocused)
                    SmallViewSearchBox.Focus(FocusState.Keyboard);
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            try
            {
                LanguageCombinations = await DictCC.GetLanguages();
                LanguageCombination = LanguageCombinations[0];
                HasAvailableLanguages = true;
                HasError = false;
            }
            catch (Exception)
            {
                HasAvailableLanguages = true;
                HasError = true;
            }
        }

        private async void Search()
        {
            String search = SearchText;

            if (search == null || search == String.Empty)
            {
                HasLanguages = false;
                Translations = null;
            }
            else
            {
                HasLanguages = true;
                try
                {
                    Translations = await DictCC.GetTranslations(LanguageCombination, search.Trim());

                    if (Translations.Translations.Count == 0)
                    {
                        ResourceLoader rl = new ResourceLoader();
                        Translations.Translations.Add(new Translation() { Destination = rl.GetString("ErrorNoTranslationsFound") });
                    }

                    HasError = false;
                }
                catch (Exception)
                {
                    HasError = true;
                }

                SimpleViewHiddenButton.Focus(FocusState.Programmatic);
                DefaultViewHiddenButton.Focus(FocusState.Programmatic);
            }
        }

        private void tbSearch_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            var s = sender as TextBox;
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Search();
                e.Handled = true;
            }
        }

        private void cbLanguages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Search();
        }

        private void ListView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (IsValidTranslation((sender as Grid).DataContext as Translation))
                FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }

        private bool IsValidTranslation(Translation t)
        {
            return t.Destination != null && t.Source != null;
        }

        private void ListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (IsValidTranslation((sender as Grid).DataContext as Translation))
                FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement f = sender as FrameworkElement;
            Translation t = f.DataContext as Translation;
            TextTopClipboard(t.Destination);
        }

        private void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e)
        {
            FrameworkElement f = sender as FrameworkElement;
            Translation t = f.DataContext as Translation;
            TextTopClipboard(t.Source);
        }

        private void TextTopClipboard(String text)
        {
            DataPackage p = new DataPackage();
            p.SetText(text);
            Clipboard.SetContent(p);
            Clipboard.Flush();
        }
        */
    }
}
