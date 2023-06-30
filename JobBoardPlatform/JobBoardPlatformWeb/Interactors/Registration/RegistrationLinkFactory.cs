using JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Interactors.Registration
{
    public class RegistrationLinkFactory : IRegistrationLinkFactory
    {
        private readonly IHttpContextAccessor contextAccessor;


        public RegistrationLinkFactory(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public string CreateConfirmationLink(string tokenId)
        {
            var request = contextAccessor.HttpContext.Request;
            var routeName = (contextAccessor.HttpContext.GetEndpoint() as RouteEndpoint)!.RoutePattern.RawText;
            return $"{request.Scheme}://{request.Host.Value}/{routeName}/confirm/{tokenId}";
        }
    }
}
