using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Factories.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Users;
using JobBoardPlatform.PL.ViewModels.Models.Profile.Employee;
using System.Security.Claims;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Factories.Applications
{
    public class OfferApplicationUpdateViewModelFactory : IViewModelAsyncFactory<OfferApplicationUpdateViewModel>
    {
        private readonly ClaimsPrincipal user;
        private readonly IRepository<EmployeeIdentity> identityRepository;
        private readonly IRepository<EmployeeProfile> profileRepository;


        public OfferApplicationUpdateViewModelFactory(ClaimsPrincipal user, 
            IRepository<EmployeeIdentity> identityRepository,
            IRepository<EmployeeProfile> profileRepository)
        {
            this.user = user;
            this.identityRepository = identityRepository;
            this.profileRepository = profileRepository;
        }

        public async Task<OfferApplicationUpdateViewModel> CreateAsync()
        {
            var update = new OfferApplicationUpdateViewModel();

            // Auto fill form
            int identityId = UserSessionUtils.GetIdentityId(user);

            var identity = await identityRepository.Get(identityId);
            var profile = await profileRepository.Get(identity.ProfileId);

            update.FullName = $"{profile.Name} {profile.Surname}";
            update.Email = identity.Email;

            var attachedResume = new EmployeeAttachedResumeViewModel();
            attachedResume.ResumeUrl = profile.ResumeUrl;
            update.AttachedResume = attachedResume;

            if (profile.Description != null)
            {
                update.AdditionalInformation = profile.Description;
            }

            return update;
        }
    }
}
