using JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens;

namespace JobBoardPlatform.PL.Interactors.Registration
{
    public class ConfirmationLinkFactory : IConfirmationLinkFactory
    {
        private readonly IHttpContextAccessor contextAccessor;


        public ConfirmationLinkFactory(IHttpContextAccessor contextAccessor)
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
