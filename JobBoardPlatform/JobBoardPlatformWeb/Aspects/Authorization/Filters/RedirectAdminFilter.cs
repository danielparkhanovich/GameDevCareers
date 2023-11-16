using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.PL.Controllers.Profile;
using JobBoardPlatform.PL.Controllers.Presenters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JobBoardPlatform.PL.Filters
{
    public class RedirectAdminFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (UserSessionUtils.IsLoggedIn(context.HttpContext.User) && 
                UserRolesUtils.IsUserAdmin(context.HttpContext.User))
            {
                context.Result = new RedirectToActionResult(
                    "Panel", 
                    ControllerUtils.GetControllerName(typeof(AdminPanelOffersController)), 
                    null);
            }
        }
    }
}
