
using JobBoardPlatform.BLL.DTOs;

namespace JobBoardPlatform.BLL.Commands.Profile
{
    public interface IUpdateUserCommandFactory
    {
        ICommand GetCommand(Type identityType, int id, ProfileData profileData);
    }
}
