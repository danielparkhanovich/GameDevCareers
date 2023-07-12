using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Models.Offer.Company
{
    public class OfferDetailsViewModel : IOfferSalary, IMainTechnology, ITechKeywords, INewOfferData
    {
        public int OfferId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int WorkLocationType { get; set; }
        public string JobDescription { get; set; } = string.Empty;
        public int ApplicationsContactType { get; set; }
        public string? ApplicationsContactEmail { get; set; }
        public int MainTechnologyType { get; set; } = 1;
        public int[] EmploymentTypes { get; set; }
        public int[]? SalaryFromRange { get; set; }
        public int[]? SalaryToRange { get; set; }
        public int[]? SalaryCurrencyType { get; set; }
        public string[]? TechKeywords { get; set; }
        public string? Street { get; set; }
        public string? ApplicationsContactExternalFormUrl { get; set; }
        public string InformationClause { get; set; } = "Informujemy, że administratorem danych jest _______ z siedzibą w _______, ul. _______(dalej jako \"administrator\"). Masz prawo do żądania dostępu do swoich danych osobowych, ich sprostowania, usunięcia lub ograniczenia przetwarzania, prawo do wniesienia sprzeciwu wobec przetwarzania, a także prawo do przenoszenia danych oraz wniesienia skargi do organu nadzorczego. Dane osobowe przetwarzane będą w celu realizacji procesu rekrutacji. Podanie danych w zakresie wynikającym z ustawy z dnia 26 czerwca 1974 r. Kodeks pracy jest obowiązkowe. W pozostałym zakresie podanie danych jest dobrowolne. Odmowa podania danych obowiązkowych może skutkować brakiem możliwości przeprowadzenia procesu rekrutacji. Administrator przetwarza dane obowiązkowe na podstawie ciążącego na nim obowiązku prawnego, zaś w zakresie danych dodatkowych podstawą przetwarzania jest zgoda. Dane osobowe będą przetwarzane do czasu zakończenia postępowania rekrutacyjnego i przez okres możliwości dochodzenia ewentualnych roszczeń, a w przypadku wyrażenia zgody na udział w przyszłych postępowaniach rekrutacyjnych - do czasu wycofania tej zgody. Zgoda na przetwarzanie danych osobowych może zostać wycofana w dowolnym momencie. Odbiorcą danych jest serwis  oraz inne podmioty, którym powierzyliśmy przetwarzanie danych w związku z rekrutacją.";
        public bool IsDisplayConsentForFutureRecruitment { get; set; }
        public string? ConsentForFutureRecruitmentContent { get; set; } = "Wyrażam zgodę na przetwarzanie moich danych osobowych dla celów przyszłych rekrutacji.";
        public bool IsDisplayCustomConsent { get; set; }
        public string? CustomConsentTitle { get; set; }
        public string? CustomConsentContent { get; set; }
    }
}
