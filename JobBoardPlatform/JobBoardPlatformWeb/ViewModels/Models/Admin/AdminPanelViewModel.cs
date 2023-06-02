using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Templates;

namespace JobBoardPlatform.PL.ViewModels.Models.Admin
{
    public interface IAdminPanelViewModel<T> where T : IEntity
    {
        public CardsContainerViewModel CardsContainer { get; set; }
        public List<T> AllRecords { get; set; }
        public int CountToGenerate { get; set; }
    }
}
