using JobBoardPlatform.DAL.Models.Enums;

namespace JobBoardPlatform.BLL.Common.Formatter
{
    public class WorkLocationTypeFormatter : ITextFormatter<string>
    {
        public string GetString(string locationType)
        {
            if (locationType == WorkLocationTypeEnum.FullyRemote.ToString())
            {
                return "Fully Remote";
            }
            else if (locationType == WorkLocationTypeEnum.Hybrid.ToString())
            {
                return "Hybrid";
            }
            else if (locationType == WorkLocationTypeEnum.OnSite.ToString())
            {
                return "On-site";
            }
            throw new Exception("Unknown location type");
        }
    }
}
