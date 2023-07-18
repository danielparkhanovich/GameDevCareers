namespace JobBoardPlatform.BLL.Boundaries
{
    public interface ICompanyProfileAndNewOfferData
    {
        ICompanyProfileData CompanyProfileData { get; set; }
        INewOfferData OfferData { get; set; }
    }
}
