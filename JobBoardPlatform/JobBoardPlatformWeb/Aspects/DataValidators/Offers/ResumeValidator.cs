using FluentValidation;
using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using JobBoardPlatform.PL.Aspects.DataValidators.Common;

namespace JobBoardPlatform.PL.Aspects.DataValidators.Offers
{
    public class ResumeValidator : AbstractValidator<IAttachedResume>
    {
        public ResumeValidator(IProfileResumeBlobStorage resumeStorage)
        {
            When(resume => string.IsNullOrEmpty(resume.ResumeUrl), () =>
            {
                RuleFor(resume => resume.File).NotNull().WithMessage("Please attach resume");
            });

            AddRuleForStorageUrlValidation(resumeStorage);

            When(resume => resume != null && resume.File != null, () =>
            {
                RuleFor(resume => resume.File!).SetValidator(GetFileValidator());
            });
        }

        private void AddRuleForStorageUrlValidation(IProfileResumeBlobStorage resumeStorage)
        {
            When(resume => !string.IsNullOrEmpty(resume.ResumeUrl), () =>
            {
                RuleFor(resume => resume.ResumeUrl).CustomAsync(async (resumeUrl, context, ct) =>
                {
                    bool isExists = await resumeStorage.IsExistsAsync(resumeUrl);
                    if (!isExists && context.InstanceToValidate.File == null)
                    {
                        context.AddFailure("Please attach resume");
                    }
                });
            });
        }

        private FileValidator GetFileValidator()
        {
            return new FileValidator(GlobalLimits.MaximumResumeSizeInMb, new string[] { "application/pdf" });
        }
    }
}
