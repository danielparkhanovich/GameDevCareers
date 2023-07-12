using FluentValidation;
using JobBoardPlatform.BLL.Boundaries;

namespace JobBoardPlatform.PL.Aspects.DataValidators.Profile
{
    public class ProfileImageValidator : AbstractValidator<IProfileImage>
    {
        public ProfileImageValidator()
        {
            RuleFor(profileImage => profileImage.File).NotNull().WithMessage("Please attach company profile image");
			AddRulesForFile();
		}

        private void AddRulesForFile()
        {
			int maxFileSizeInBytes = GlobalLimits.GetValueInBytesFromMb(GlobalLimits.MaximumProfileImageSizeInMb);
			RuleFor(profileImage => profileImage.File!.Length).LessThan(maxFileSizeInBytes)
                .WithMessage($"File cannot be larger than {GlobalLimits.MaximumProfileImageSizeInMb} MB");

            string[] availableFormats = new string[] { "image/jpeg", "image/png" };
            RuleFor(profileImage => profileImage.File!.ContentType).Must(x => availableFormats.Contains(x))
                .WithMessage($"Profile image must be in JPG or PNG format");
		}
    }
}
