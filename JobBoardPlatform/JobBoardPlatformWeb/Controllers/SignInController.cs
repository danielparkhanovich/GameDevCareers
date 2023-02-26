using JobBoardPlatform.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers
{
    public class SignInController : Controller
    {
        public IActionResult Employee()
        {
            return View();
        }

        public IActionResult Company()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Employee(UserLoginViewModel userLogin)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction();
            }
            return View(userLogin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Company(UserLoginViewModel userLogin)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction();
            }
            return View(userLogin);
        }
    }
}
