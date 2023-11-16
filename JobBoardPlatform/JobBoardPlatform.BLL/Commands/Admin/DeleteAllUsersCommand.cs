using JobBoardPlatform.BLL.Commands.Identity;
using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Commands.Admin
{
    public class DeleteAllUsersCommand<T> : ICommand
        where T : class, IUserIdentityEntity
    {
        private readonly UserManager<T> userManager;


        public DeleteAllUsersCommand(UserManager<T> userManager)
        {
            this.userManager = userManager;
        }

        public async Task Execute()
        {
            var allRecords = await userManager.GetAllAsync();
            foreach (var offer in allRecords)
            {
                await userManager.DeleteAsync(offer.Id);
            }
        }
    }
}
