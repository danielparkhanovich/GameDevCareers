using JobBoardPlatform.PL.Filters;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.RestorePassword
{
    [TypeFilter(typeof(SkipLoggedInUsersFilter))]
    [Route("restore")]
    public class RestorePasswordController : Controller
    {
        public RestorePasswordController()
        {

        }

        public IActionResult Restore()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(string email)
        {
            if (ModelState.IsValid)
            {

            }
            return View();
        }

        [Route("{tokenId}")]
        public async Task<IActionResult> RedirectToChangePassword(string email)
        {
            // 
            return Redirect(email);
        }
    }
}
