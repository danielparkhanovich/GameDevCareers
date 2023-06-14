using System.Globalization;

namespace JobBoardPlatform.PL.Localization.Text
{
    public sealed class LocalizationManager
    {
        public static LocalizationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LocalizationManager();
                }
                return instance;
            }
        }
        private static LocalizationManager instance = null;

        private readonly Dictionary<LanguageType, string> languageCultures = new Dictionary<LanguageType, string>()
        {
            { LanguageType.English, "en-US" },
            { LanguageType.Polish, "pl-PL" }
        };

        private LanguageType currentLanguage;


        private LocalizationManager()
        {
            SetLanguage(LanguageType.English);
        }

        public void SetLanguage(LanguageType language)
        {
            currentLanguage = language;

            string userLanguage = languageCultures[language];
            Thread.CurrentThread.CurrentCulture = new CultureInfo(userLanguage);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(userLanguage);
        }

        #region TRANSLATION_PROPERTIES

        public string TEST => @Resource.Button_Search_Text;

        #endregion
    }
}
