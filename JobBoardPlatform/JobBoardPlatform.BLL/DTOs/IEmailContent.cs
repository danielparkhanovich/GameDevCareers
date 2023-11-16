namespace JobBoardPlatform.BLL.DTOs
{
    public interface IEmailContent<T>
    {
        Task<string> GetSubjectAsync(T value);
        Task<string> GetMessageAsync(T value);
    }
}
