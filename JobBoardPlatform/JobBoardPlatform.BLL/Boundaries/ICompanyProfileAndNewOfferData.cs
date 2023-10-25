namespace JobBoardPlatform.BLL.Boundaries
{
    public interface ICompanyProfileAndNewOfferData
    {
        CompanyProfileData CompanyProfileData { get; set; }
        IOfferData OfferData { get; set; }
    }
}
