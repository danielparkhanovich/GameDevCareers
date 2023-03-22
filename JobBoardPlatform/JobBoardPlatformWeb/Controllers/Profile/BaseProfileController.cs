using JobBoardPlatform.BLL.Services.Authorization;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Contracts;
using JobBoardPlatform.PL.ViewModels.Profile;
using JobBoardPlatform.PL.ViewModels.Profile.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Profile
{
    [Authorize]
    public abstract class BaseProfileController<T, V> : Controller
        where T : class, IEntity
        where V : class, IProfileViewModel
    {
        protected IRepository<T> profileRepository;


        public abstract Task<IActionResult> Profile();

        public abstract Task<IActionResult> Profile(V userViewModel);
    }
}
