using EasyDictionary.Domain;
using EasyDictionary.Services;
using EasyDictionary.Views;
using SplitViewMenu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

namespace EasyDictionary.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        const String FontAwesome = @"ms-appx:///Assets/FontAwesome.otf#FontAwesome";

        private Type _initialPage = typeof(TranslationPage);
        public Type InitialPage
        {
            get { return _initialPage; }
            set
            {
                if (_initialPage == value) return;
                _initialPage = value;
                NotifyPropertyChanged();
            }
        }

        private IEnumerable<INavigationMenuItem> _menuItems;
        public IEnumerable<INavigationMenuItem> MenuItems
        {
            get { return _menuItems; }
            set
            {
                if (_menuItems == value) return;
                _menuItems = value;
                NotifyPropertyChanged();
            }
        }

        private IEnumerable<INavigationMenuItem> _menuItemsBottom;
        public IEnumerable<INavigationMenuItem> MenuItemsBottom
        {
            get { return _menuItemsBottom; }
            set
            {
                if (_menuItemsBottom == value) return;
                _menuItemsBottom = value;
                NotifyPropertyChanged();
            }
        }

        public MainPageViewModel()
        {
            var rl = new ResourceLoader();
            _menuItems = new List<INavigationMenuItem>()
            {
                new SimpleNavMenuItem()
                {
                    DestinationPage = typeof(TranslationPage),
                    Label = rl.GetString("PageTitleTranslate"),
                    FontFamily = FontAwesome,
                    Symbol = "\uf0ac"
                }
            };

            _menuItemsBottom = new List<INavigationMenuItem>()
            {
                new SimpleNavMenuItem()
                {
                    DestinationPage = typeof(AboutPage),
                    Label = rl.GetString("PageTitleAbout"),
                    FontFamily = FontAwesome,
                    Symbol = "\uf05a"
                }
            };
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
