using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;

namespace JobBoardPlatform.PL.Controllers.Login
{
    public class CompanyLoginController : BaseLoginController<CompanyIdentity, CompanyProfile>
    {
        public CompanyLoginController(ILoginService<CompanyIdentity, CompanyProfile> loginService) : base(loginService)
        {
            
        }
    }
}
