using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace EasyDictionary.Exceptions
{
    public interface ITranslatableException
    {
        string TranslationKey { get; }
        string Translate(ResourceLoader loader);
    }

    public abstract class TranslatableException : Exception, ITranslatableException
    {
        private string _translationKey = String.Empty; 
        public string TranslationKey
        {
            get { return _translationKey; }
        }

        public TranslatableException(string translationKey)
        {
            _translationKey = translationKey;
        }

        public virtual string Translate(ResourceLoader loader)
        {
            return loader.GetString(_translationKey);
        }
    }

    public class UnparsableLanguageInformationException : TranslatableException
    {
        public UnparsableLanguageInformationException(): base("ErrorBackendFormatHasChanged") { }
    }

    public class UnparsableTranslationException : TranslatableException
    {
        public UnparsableTranslationException(): base("ErrorBackendFormatHasChanged") { }
    }

    public class NoLanguagesFoundException : TranslatableException
    {
        public NoLanguagesFoundException(): base("ErrorNotLanguagesFound") { }
    }

    public class ServiceNotReachableException : TranslatableException
    {
        private string _serviceName;
        public ServiceNotReachableException(String serviceName): base("ErrorBackendServiceNotReachable")
        {
            _serviceName = serviceName;
        }

        public override string Translate(ResourceLoader resourceLoader)
        {
            return String.Format(resourceLoader.GetString(TranslationKey), _serviceName);
        }
    }

    public class UnknownErrorException: TranslatableException
    {
        public UnknownErrorException(): base("ErrorUnknown") { }
    }
}
