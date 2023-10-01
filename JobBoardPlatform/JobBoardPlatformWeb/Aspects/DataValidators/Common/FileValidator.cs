using FluentValidation;

namespace JobBoardPlatform.PL.Aspects.DataValidators.Common
{
    public class FileValidator : AbstractValidator<IFormFile>
    {
        public FileValidator(int maxFileSizeInMb, string[] availableFormats)
        {
            RuleFor(file => file.Length).LessThan(GlobalLimits.GetValueInBytesFromMb(maxFileSizeInMb))
                .WithMessage($"File cannot be larger than {maxFileSizeInMb} MB");

            RuleFor(file => file.ContentType).Must(x => availableFormats.Contains(x))
                .WithMessage($"Profile image must be in {GetFormatsString(availableFormats)} format");
        }

        private string GetFormatsString(string[] availableFormats)
        {
            var readableFormats = new List<string>();
            foreach (var format in availableFormats)
            {
                // format example: application/pdf
                var tokens = format.Split('/');
                if (tokens.Length == 2)
                {
                    readableFormats.Add(tokens[1].ToUpper());
                }
                else
                {
                    readableFormats.Add(tokens[0]);
                }
            }

            return string.Join(" or ", readableFormats);
        }
    }
}
