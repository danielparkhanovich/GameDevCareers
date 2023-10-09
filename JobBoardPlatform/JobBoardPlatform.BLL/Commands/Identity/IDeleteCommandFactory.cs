using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Commands.Identities
{
    public interface IDeleteCommandFactory
    {
        ICommand GetCommand(Type identityType, int id);
    }
}
