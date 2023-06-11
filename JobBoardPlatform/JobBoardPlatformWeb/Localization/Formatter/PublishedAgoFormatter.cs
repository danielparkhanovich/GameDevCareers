namespace JobBoardPlatform.BLL.Common.Formatter
{
    public class PublishedAgoFormatter : ITextFormatter<DateTime>
    {
        private readonly bool isPublished;


        public PublishedAgoFormatter(bool isPublished)
        {
            this.isPublished = isPublished;
        }

        public string GetString(DateTime date)
        {
            int daysAgo = (DateTime.Now - date).Days;
            if (daysAgo == 0)
            {
                return "new";
            }

            if (isPublished)
            {
                return $"{daysAgo}d ago";
            }
            else
            {
                return $"{daysAgo}d ago";
            }
        }
    }
}
