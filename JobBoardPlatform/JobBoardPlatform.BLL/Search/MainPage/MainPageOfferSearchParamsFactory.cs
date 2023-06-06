using JobBoardPlatform.BLL.Search.Enums;
using JobBoardPlatform.BLL.Search.Templates;
using JobBoardPlatform.DAL.Models.Enums;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Search.MainPage
{
    /// <summary>
    /// Create offer search data based on current url
    /// </summary>
    public class MainPageOfferSearchParamsFactory : SearchParamsUrlFactoryBase<MainPageOfferSearchParams>
    {
        protected override void AssignFilterParams(HttpRequest httpRequest, MainPageOfferSearchParams searchParams)
        {
            var tabString = httpRequest.Path.ToString().ToLower();
            searchParams.Type = GetOfferType(tabString);

            // Get all query parameters
            string? technologyString = httpRequest.Query[OfferSearchUrlParams.Technology];
            searchParams.MainTechnology = GetMainTechnology(technologyString);

            searchParams.IsSalaryOnly = IsBoolFilter(httpRequest, OfferSearchUrlParams.SalaryOnly);
            searchParams.IsRemoteOnly = IsBoolFilter(httpRequest, OfferSearchUrlParams.RemoteOnly);
            searchParams.SearchString = httpRequest.Query[OfferSearchUrlParams.Search];
        }

        private OfferType GetOfferType(string typeString)
        {
            OfferType offerType = OfferType.Employment;

            if (typeString == "/commissions")
            {
                offerType = OfferType.Commission;
            }

            return offerType;
        }

        private int GetMainTechnology(string? technology)
        {
            if (string.IsNullOrEmpty(technology))
            {
                return MainPageOfferSearchParams.AllTechnologiesIndex;
            }

            technology = technology.ToLower();

            var technologies = Enum.GetValues(typeof(MainTechnologyTypeEnum))
                .Cast<MainTechnologyTypeEnum>();

            if (!technologies.Any(x => x.ToString().ToLower() == technology))
            {
                return MainPageOfferSearchParams.AllTechnologiesIndex;
            }

            var foundIndex = technologies.FirstOrDefault(x => x.ToString().ToLower() == technology);

            return (int)foundIndex + 1;
        }
    }
}
