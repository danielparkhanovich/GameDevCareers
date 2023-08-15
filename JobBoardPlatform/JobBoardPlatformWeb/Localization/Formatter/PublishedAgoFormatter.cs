namespace JobBoardPlatform.BLL.Common.Formatter
{
    public class PublishedAgoFormatter : ITextFormatter<DateTime>
    {
        public PublishedAgoFormatter()
        {
        }

        public string GetString(DateTime date)
        {
            int daysAgo = (DateTime.Now - date).Days;
            if (daysAgo == 0)
            {
                return "new";
            }

            return $"{daysAgo}d ago";
        }
    }
}
