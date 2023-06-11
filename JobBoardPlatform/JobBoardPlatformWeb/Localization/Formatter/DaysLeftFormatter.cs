namespace JobBoardPlatform.BLL.Common.Formatter
{
    public class DaysLeftFormatter : ITextFormatter<int>
    {
        public string GetString(int daysLeft)
        {
            string daysLeftLabel = string.Empty;
            if (daysLeft <= 0)
            {
                daysLeftLabel = "expired";
            }
            else
            {
                daysLeftLabel = $"{daysLeft}d left";
            }

            return daysLeftLabel;
        }
    }
}
