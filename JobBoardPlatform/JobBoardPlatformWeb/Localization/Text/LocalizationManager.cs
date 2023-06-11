namespace JobBoardPlatform.PL.Localization.Text
{
    public class LocalizationManager
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

        private static LocalizationManager instance;


        private LocalizationManager()
        {

        }

        public void SetLanguage()
        {

        }
    }
}
