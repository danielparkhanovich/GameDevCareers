using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Services.Authentification.Contracts
{
    internal interface IIdentityValidator<T>
        where T : class, IUserIdentityEntity
    {
        void ValidateRegisterAsync(T? user);
        void ValidateLoginAsync(T? user, string hashedPassword);
    }
}
