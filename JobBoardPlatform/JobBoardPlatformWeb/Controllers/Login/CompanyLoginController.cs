using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Login
{
    [Route("signin-employer-panel")]
    public class CompanyLoginController : BaseLoginController<CompanyIdentity, CompanyProfile>
    {
        public CompanyLoginController(ILoginService<CompanyIdentity, CompanyProfile> loginService) : base(loginService)
        {
            
        }
    }
}
