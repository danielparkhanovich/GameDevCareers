namespace JobBoardPlatform.BLL.Commands
{
    public interface ICommandWithResult<T> : ICommand
    {
        T Result { get; }
    }
}
