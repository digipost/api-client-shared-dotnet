using System;
using System.Reflection;
using System.Resources;

namespace Digipost.Api.Client.Shared.Resources.Language
{
    public class LanguageResource
    {
        private const string ResourceBasePath = "Digipost.Api.Client.Shared.Resources.Language.Data";
        private static readonly string EnUs = $"{ResourceBasePath}.en-us";
        private static readonly string NbNo = $"{ResourceBasePath}.nb-no";
        private static readonly ResourceManager EnglishResourceManager = new ResourceManager(EnUs, Assembly.GetExecutingAssembly());
        private static readonly ResourceManager NorwegianResourceManager = new ResourceManager(NbNo, Assembly.GetExecutingAssembly());
        
        public static Language CurrentLanguage { get; set; } = Language.English;

        public static string GetResource(LanguageResourceKey key)
        {
            return GetResource(key, CurrentLanguage);
        }

        public static string GetResource(LanguageResourceKey key, Language language)
        {
            return GetManagerForLanguage(language).GetString(key.ToString());
        }

        private static ResourceManager GetManagerForLanguage(Language language)
        {
            switch (language)
            {
                case Language.English:
                    return EnglishResourceManager;
                case Language.Norwegian:
                    return NorwegianResourceManager;
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, null);
            }
        }
    }
}