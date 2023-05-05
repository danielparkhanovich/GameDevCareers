using JobBoardPlatform.BLL.Common.Contracts;
using JobBoardPlatform.DAL.Models.Enums;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Search.Offers
{
    /// <summary>
    /// Create offer search data based on current url
    /// </summary>
    public class OfferSearchDataUrlFactory : IFactory<OfferSearchData>
    {
        private readonly HttpRequest request;


        public OfferSearchDataUrlFactory(HttpRequest request)
        {
            this.request = request;
        }

        public OfferSearchData Create()
        {
            string tabString = request.Path.ToString().ToLower();
            OfferType offerType = GetOfferType(tabString);

            // Get all query parameters
            string? technologyString = request.Query[OfferSearchUrlParameters.Technology];
            int mainTechnology = GetMainTechnology(technologyString);

            bool isSalaryOnly = request.Query.ContainsKey(OfferSearchUrlParameters.SalaryOnly);
            bool isRemoteOnly = request.Query.ContainsKey(OfferSearchUrlParameters.RemoteOnly);
            string? searchString = request.Query[OfferSearchUrlParameters.SearchString];

            int page = int.TryParse(OfferSearchUrlParameters.Page, out int intOutParameter) ? intOutParameter : 1;

            return new OfferSearchData()
            {
                Type = offerType,
                MainTechnology = mainTechnology,
                IsRemoteOnly = isRemoteOnly,
                IsSalaryOnly = isSalaryOnly,
                SearchString = searchString,
                Page = page,
            };
        }

        private OfferType GetOfferType(string typeString)
        {
            OfferType offerType = OfferType.Employment;

            if (typeString == OfferType.Commission.ToString().ToLower())
            {
                offerType = OfferType.Commission;
            }

            return offerType;
        }

        private int GetMainTechnology(string? technologyString)
        {
            // 0 -> any
            if (string.IsNullOrEmpty(technologyString))
            {
                return 0;
            }

            var technologies = Enum.GetValues(typeof(MainTechnologyTypeEnum))
                .Cast<MainTechnologyTypeEnum>();

            var foundIndex = technologies
                .FirstOrDefault(x => x.ToString().ToLower() == technologyString);

            return (int)foundIndex;
        }
    }
}
