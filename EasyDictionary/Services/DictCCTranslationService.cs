using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyDictionary.Domain;
using System.Text.RegularExpressions;
using EasyDictionary.Exceptions;
using Windows.Globalization;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace EasyDictionary.Services
{
    public class DictCCTranslationService : ITranslationService
    {
        private const String URL_BASE = "http://www.dict.cc/";
        private const String URL_SEARCH = "http://{0}{1}.dict.cc/?s={2}";

        public async Task<IEnumerable<TranslationLanguageOption>> LoadTranslationOptions()
        {
            List<TranslationLanguageOption> result = new List<TranslationLanguageOption>();
            string html = "";

            try
            {
                html = await GetHtml(new Uri(URL_BASE));
            }
            catch (Exception)
            {
                throw new ServiceNotReachableException(URL_BASE);
            }

            // Filter languages
            Regex regex = new Regex("<option value=\"([A-Z]{4})\">");
            HashSet<String> langs = new HashSet<string>();
            foreach (Match match in regex.Matches(html))
            {
                String tmp = match.Groups[1].Value;

                if (!langs.Contains(tmp))
                {
                    String sourceLanguage = tmp.Substring(0, 2).ToLower();
                    String destinationLanguage = tmp.Substring(2, 2).ToLower();

                    if (sourceLanguage == String.Empty || destinationLanguage == String.Empty)
                        throw new UnparsableLanguageInformationException();

                    TranslationLanguageOption comb = new TranslationLanguageOption();
                    comb.Source = new Language(sourceLanguage);
                    comb.Destination = new Language(destinationLanguage);

                    if (!String.IsNullOrEmpty(comb.Destination.DisplayName) && 
                        !String.IsNullOrEmpty(comb.Source.DisplayName))
                    {
                        result.Add(comb);
                        langs.Add(tmp);
                    }
                }
            }

            if (result.Count == 0)
                throw new NoLanguagesFoundException();

            return result;
        }

        public async Task<IEnumerable<Translation>> LoadTranslations(TranslationLanguageOption translationOption, string search)
        {
            if (String.IsNullOrEmpty(search) || 
                translationOption == null)
                return new List<Translation>();

            List<Translation> words = new List<Translation>();
            TranslationRequest result = new TranslationRequest()
            {
                Option = translationOption,
                Search = search,
                Translations = words
            };

            // http://defr.dict.cc/?s=hallo
            String url = String.Format(
                URL_SEARCH,
                translationOption.Source.LanguageTag.ToLower(),
                translationOption.Destination.LanguageTag.ToLower(),
                Uri.EscapeDataString(search));


            String html = "";

            try
            {
                html = await GetHtml(new Uri(url));
            }
            catch (Exception)
            {
                throw new ServiceNotReachableException(URL_BASE);
            }

            // Filter languages
            Regex regexLang1 = new Regex("c1Arr.*Array\\((?:(?:\"(.*?)\",{0,1}){0,})");
            Regex regexLang2 = new Regex("c2Arr.*Array\\((?:(?:\"(.*?)\",{0,1}){0,})");
            Regex regexNothingFound = new Regex("No entries found!");

            if (regexNothingFound.IsMatch(html))
                return words;

            Match matchLang1 = regexLang1.Match(html);
            Match matchLang2 = regexLang2.Match(html);

            if (!matchLang1.Success || !matchLang2.Success)
                throw new UnparsableTranslationException();

            CaptureCollection groupLang1 = matchLang1.Groups[1].Captures;
            CaptureCollection groupLang2 = matchLang2.Groups[1].Captures;

            // Loop in a task
            for (int i = 0; i < groupLang1.Count; i++)
            {
                String s1 = groupLang1[i].Value.Replace(@"\'", "'");
                String s2 = groupLang2[i].Value.Replace(@"\'", "'");

                if (s1 == String.Empty || s2 == String.Empty) continue;

                words.Add(new Translation() { Source = s1, Destination = s2 });
            }

            return words;
        }

        private async static Task<String> GetHtml(Uri uri)
        {
            String result = String.Empty;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
                result = await client.GetStringAsync(uri);
            }
            return result;
        }
    }
}
