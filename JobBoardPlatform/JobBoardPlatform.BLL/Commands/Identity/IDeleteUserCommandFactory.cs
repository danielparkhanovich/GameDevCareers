
namespace JobBoardPlatform.BLL.Commands.Identities
{
    public interface IDeleteUserCommandFactory
    {
        ICommand GetCommand(Type identityType, int id);
    }
}
