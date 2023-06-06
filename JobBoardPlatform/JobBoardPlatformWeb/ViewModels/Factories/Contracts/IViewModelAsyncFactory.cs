namespace JobBoardPlatform.PL.ViewModels.Factories.Contracts
{
    public interface IViewModelAsyncFactory<T>
    {
        Task<T> CreateAsync();
    }
}
