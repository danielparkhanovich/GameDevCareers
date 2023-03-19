using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        [Authorize(Policy = AuthorizationPolicies.USER_ONLY_POLICY)]
        public IActionResult Employee()
        {
            return View();
        }

        [Authorize(Policy = AuthorizationPolicies.COMPANY_ONLY_POLICY)]
        public IActionResult Company()
        {
            return View();
        }
    }
}
