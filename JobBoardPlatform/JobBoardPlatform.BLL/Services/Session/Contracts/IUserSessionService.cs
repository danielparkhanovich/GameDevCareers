namespace JobBoardPlatform.BLL.Services.Session.Contracts
{
    public interface IUserSessionService<T>
    {
        Task UpdateSessionStateAsync(T profile);
    }
}
