using JobBoardPlatform.DAL.Models.Enums;

namespace JobBoardPlatform.BLL.Common.Formatter
{
    public class EmploymentTypeFormatter : ITextFormatter<string>
    {
        public string GetString(string employmentType)
        {
            if (employmentType == EmploymentTypeEnum.Permanent.ToString())
            {
                return "Permanent";
            }
            else if (employmentType == EmploymentTypeEnum.MandateContract.ToString())
            {
                return "Mandate Contract";
            }
            else if (employmentType == EmploymentTypeEnum.B2B.ToString())
            {
                return "B2B";
            }
            throw new Exception("Unknown employment type");
        }
    }
}
