
namespace JobBoardPlatform.PL.ViewModels.Factories.Contracts
{
    public interface IViewModelFactory<TEntity, TViewModel>
        where TViewModel : class
    {
        TViewModel Create(TEntity data);
    }
}
