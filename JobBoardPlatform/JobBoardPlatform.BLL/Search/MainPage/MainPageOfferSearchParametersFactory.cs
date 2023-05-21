using JobBoardPlatform.BLL.Common.Contracts;
using JobBoardPlatform.BLL.Search.Enums;
using JobBoardPlatform.DAL.Models.Enums;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Search.MainPage
{
    /// <summary>
    /// Create offer search data based on current url
    /// </summary>
    public class MainPageOfferSearchParametersFactory : IFactory<MainPageOfferSearchParameters>
    {
        private readonly HttpRequest request;


        public MainPageOfferSearchParametersFactory(HttpRequest request)
        {
            this.request = request;
        }

        public MainPageOfferSearchParameters Create()
        {
            string tabString = request.Path.ToString().ToLower();
            OfferType offerType = GetOfferType(tabString);

            // Get all query parameters
            string? technologyString = request.Query[OfferSearchUrlParameters.Technology];
            int mainTechnology = GetMainTechnology(technologyString);

            bool isSalaryOnly = request.Query.ContainsKey(OfferSearchUrlParameters.SalaryOnly);
            bool isRemoteOnly = request.Query.ContainsKey(OfferSearchUrlParameters.RemoteOnly);
            string? searchString = request.Query[OfferSearchUrlParameters.Search];
            string? pageString = request.Query[OfferSearchUrlParameters.Page];

            int page = int.TryParse(pageString, out int intOutParameter) ? intOutParameter : 1;

            return new MainPageOfferSearchParameters()
            {
                Type = offerType,
                MainTechnology = mainTechnology,
                IsRemoteOnly = isRemoteOnly,
                IsSalaryOnly = isSalaryOnly,
                SearchString = searchString,
                Page = page,
                PageSize = 20
            };
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
                return MainPageOfferSearchParameters.AllTechnologiesIndex;
            }

            technology = technology.ToLower();

            var technologies = Enum.GetValues(typeof(MainTechnologyTypeEnum))
                .Cast<MainTechnologyTypeEnum>();

            if (!technologies.Any(x => x.ToString().ToLower() == technology))
            {
                return MainPageOfferSearchParameters.AllTechnologiesIndex;
            }

            var foundIndex = technologies.FirstOrDefault(x => x.ToString().ToLower() == technology);

            return (int)foundIndex + 1;
        }
    }
}
