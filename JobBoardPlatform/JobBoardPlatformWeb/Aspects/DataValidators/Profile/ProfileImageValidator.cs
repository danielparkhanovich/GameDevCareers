using FluentValidation;
using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.PL.Aspects.DataValidators.Common;

namespace JobBoardPlatform.PL.Aspects.DataValidators.Profile
{
    public class ProfileImageValidator : AbstractValidator<IProfileImage>
    {
        public ProfileImageValidator()
        {
            When(profileImage => string.IsNullOrEmpty(profileImage.ImageUrl), () =>
            {
                RuleFor(profileImage => profileImage.File).NotNull().WithMessage("Please attach profile image");
            });

            When(profileImage => profileImage != null && profileImage.File != null, () => 
            {
                RuleFor(profileImage => profileImage.File!).SetValidator(GetFileValidator());
            });
		}

        private FileValidator GetFileValidator()
        {
			return new FileValidator(GlobalLimits.MaximumProfileImageSizeInMb, new string[] { "image/jpeg", "image/png" });
        }
    }
}
