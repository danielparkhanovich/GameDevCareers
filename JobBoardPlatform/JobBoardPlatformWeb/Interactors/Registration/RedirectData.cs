namespace JobBoardPlatform.PL.Interactors.Registration
{
    public class RedirectData
    {
        public static RedirectData NoRedirect = new RedirectData();

        public string ActionName { get; set; }
        public object Data { get; set; }
    }
}
