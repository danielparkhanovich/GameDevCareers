using FluentValidation;
using JobBoardPlatform.BLL.DTOs;
using JobBoardPlatform.PL.Aspects.DataValidators.Common;
using JobBoardPlatform.BLL.Common;

namespace JobBoardPlatform.PL.Aspects.DataValidators.Profile
{
    public class ProfileImageValidator : AbstractValidator<ProfileImage>
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
			return new FileValidator(GlobalBLL.Limits.MaximumProfileImageSizeInMb, new string[] { "image/jpeg", "image/png" });
        }
    }
}
