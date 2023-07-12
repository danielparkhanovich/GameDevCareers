using FluentValidation;

namespace JobBoardPlatform.PL.Aspects.DataValidators.Common
{
    public class UrlValidator : AbstractValidator<string>
    {
        public UrlValidator()
        {
            RuleFor(url => url)
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .When(url => !string.IsNullOrEmpty(url))
                .WithMessage("Enter valid url address");
        }
    }
}
