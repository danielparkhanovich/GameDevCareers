using JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens;
using System.Text.RegularExpressions;

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
            routeName = RemoveTemplateParts(routeName);
            return $"{request.Scheme}://{request.Host.Value}/{routeName}/confirm/{tokenId}";
        }

        private string RemoveTemplateParts(string routeName)
        {
            // remove all /{...}
            var re = new Regex(@"/\{[^}]+\}");
            return re.Replace(routeName, string.Empty);
        }
    }
}
