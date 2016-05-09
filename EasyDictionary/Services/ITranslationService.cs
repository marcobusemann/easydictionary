using EasyDictionary.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyDictionary.Services
{
    public interface ITranslationService
    {
        Task<IEnumerable<TranslationLanguageOption>> LoadTranslationOptions();
        Task<IEnumerable<Translation>> LoadTranslations(TranslationLanguageOption translationOption, String search);
    }
}
