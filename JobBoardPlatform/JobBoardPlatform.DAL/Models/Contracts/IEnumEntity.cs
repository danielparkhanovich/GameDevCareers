namespace JobBoardPlatform.DAL.Models.Contracts
{
    public interface IEnumEntity : IEntity
    {
        public string Type { get; set; }
    }
}
