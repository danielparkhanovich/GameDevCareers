using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.PL.Filters;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Register
{
    [TypeFilter(typeof(SkipLoggedInUsersFilter))]
    public abstract class BaseRegisterController<T> : Controller where T : class, IUserLoginData
    {
        public const string TryConfirmRegistrationAction = "TryConfirmRegistration";
        public const string CheckVerifyingTokenAction = "CheckVerifyingToken";


        public IActionResult Register()
        {
            return View();
        }

        [Route("verifying")]
        public IActionResult CheckVerifyingToken()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public abstract Task<IActionResult> Register(T userRegister);

        [Route("confirm/{tokenId}")]
        public abstract Task<IActionResult> TryConfirmRegistration(string tokenId);

        [Route("confirm/{email}/{passwordHash}")]
        public abstract Task<IActionResult> TryConfirmRegistration(string email, string passwordHash);
    }
}
