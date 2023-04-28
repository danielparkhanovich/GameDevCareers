using JobBoardPlatform.BLL.Commands.Contracts;
using JobBoardPlatform.PL.ViewModels.Profile.Employee;

namespace JobBoardPlatform.PL.ViewModels.OfferViewModels.Users
{
    public class OfferContentViewModel : IAttachedResume
    {
        public IFormFile? File { get => Update.AttachedResume.File; set => Update.AttachedResume.File = value; }
        public string? ResumeUrl { get => Update.AttachedResume.ResumeUrl; set => Update.AttachedResume.ResumeUrl = value; }
        public string? FileName { get => Update.AttachedResume.FileName; set => Update.AttachedResume.FileName = value; }
        public string? FileSize { get => Update.AttachedResume.FileSize; set => Update.AttachedResume.FileSize = value; }

        public OfferContentDisplayViewModel Display { get; set; }
        public OfferApplicationUpdateViewModel Update { get; set; }


        public OfferContentViewModel()
        {
            Display = new OfferContentDisplayViewModel();
            Update = new OfferApplicationUpdateViewModel()
            {
                AttachedResume = new EmployeeAttachedResumeViewModel()
            };
        }
    }
}
