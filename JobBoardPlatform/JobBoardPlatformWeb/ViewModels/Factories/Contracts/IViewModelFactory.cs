using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Factories.Contracts
{
    public interface IViewModelFactory<TEntity, TViewModel>
        where TEntity : IEntity
        where TViewModel : class
    {
        TViewModel CreateViewModel(TEntity data);
    }
}
