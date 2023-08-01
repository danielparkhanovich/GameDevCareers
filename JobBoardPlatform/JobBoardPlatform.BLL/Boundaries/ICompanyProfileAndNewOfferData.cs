namespace JobBoardPlatform.BLL.Boundaries
{
    public interface ICompanyProfileAndNewOfferData
    {
        ICompanyProfileData CompanyProfileData { get; set; }
        IOfferData OfferData { get; set; }
    }
}
