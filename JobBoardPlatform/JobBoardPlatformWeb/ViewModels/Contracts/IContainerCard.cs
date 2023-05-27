namespace JobBoardPlatform.PL.ViewModels.Contracts
{
    public interface IContainerCard
    {
        public string PartialView { get; }
        public int Id { get; set; }
    }
}
