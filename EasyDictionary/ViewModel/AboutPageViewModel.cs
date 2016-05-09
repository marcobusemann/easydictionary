using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace EasyDictionary.ViewModel
{
    public class AboutPageViewModel : INotifyPropertyChanged
    {
        public Uri RepositoryUri { get { return new Uri("http://google.de"); } }
        public Uri TermsOfUseUri { get { return new Uri("http://google.de"); } }
        public Uri PrivacyUri { get { return new Uri("http://google.de"); } }

        public String Version { get; } = BuildVersionString();
        public String ApplicationName { get; } = BuildApplicationName();

        public String ApplicationVersion { get { return String.Format("{0} {1}", ApplicationName, Version); } }

        private String _copyRight;
        public String CopyRight { get { return _copyRight; } }

        private IEnumerable<KeyValuePair<String, String>> _featureDevelopers;
        public IEnumerable<KeyValuePair<String, String>> FeatureDevelopers { get { return _featureDevelopers; } }

        public AboutPageViewModel()
        {
            _copyRight = String.Format("© {0} Marco Busemann", DateTime.Now.Year);
            _featureDevelopers = new List<KeyValuePair<String, String>>()
            {
                new KeyValuePair<string, string>("Core app", "Marco Busemann")
            };
        }

        private static String BuildApplicationName()
        {
            Package package = Package.Current;
            return package.DisplayName;
        }

        private static String BuildVersionString()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;
            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
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
