using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization;

namespace EasyDictionary.Domain
{
    /// Represents an option to translate form language 1  to language 2.
    /// Actually all options are treated bidirectional.
    /// Eg.: de_DE <-> en_US
    public class TranslationLanguageOption
    {
        public Language Destination { get; set; }
        public Language Source { get; set; }

        public override string ToString()
        {
            return String.Format("{0} <-> {1}", Source.DisplayName, Destination.DisplayName); 
        }
    }

}
