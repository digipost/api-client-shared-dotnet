using System;
using System.Reflection;
using System.Resources;

namespace Difi.Felles.Utility.Resources.Language
{
    public class LanguageResource
    {
        private const string ResourceBasePath = "Digipost.Api.Client.Shared.Resources.Language.Data";
        private static readonly string EnUs = $"{ResourceBasePath}.en-us";
        private static readonly ResourceManager EnglishResourceManager = new ResourceManager(EnUs, Assembly.GetExecutingAssembly());

        public static Digipost.Api.Client.Shared.Resources.Language.Language CurrentLanguage { get; set; } = Digipost.Api.Client.Shared.Resources.Language.Language.English;

        public static string GetResource(LanguageResourceKey key)
        {
            return GetResource(key, CurrentLanguage);
        }

        public static string GetResource(LanguageResourceKey key, Digipost.Api.Client.Shared.Resources.Language.Language language)
        {
            return GetManagerForLanguage(language).GetString(key.ToString());
        }

        private static ResourceManager GetManagerForLanguage(Digipost.Api.Client.Shared.Resources.Language.Language language)
        {
            switch (language)
            {
                case Digipost.Api.Client.Shared.Resources.Language.Language.English:
                    return EnglishResourceManager;
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, null);
            }
        }
    }
}