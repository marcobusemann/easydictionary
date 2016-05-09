using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasyDictionary.Domain
{
    
    /// Represents a search request when the user entered a search string.
    public class TranslationRequest
    {
        public IEnumerable<Translation> Translations { get; set; }
        public TranslationLanguageOption Option { get; set; }
        public String Search { get; set; }
    }
}
