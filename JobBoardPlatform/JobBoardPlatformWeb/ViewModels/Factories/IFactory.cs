namespace JobBoardPlatform.PL.ViewModels.Utilities.Contracts
{
    /// <summary>
    /// Creates new view model
    /// </summary>
    /// <typeparam name="T">View Model</typeparam>
    public interface IFactory<T>
    {
        Task<T> Create();
    }
}
