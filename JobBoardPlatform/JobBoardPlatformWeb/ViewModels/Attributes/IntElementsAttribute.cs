using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.PL.ViewModels.Attributes
{
    public class IntElementsAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            object[] array = value as object[];

            if (array == null)
            {
                return true;
            }

            if (array.Any(x => x is int) == false)
            {
                return false;
            }

            return true;
        }
    }
}
