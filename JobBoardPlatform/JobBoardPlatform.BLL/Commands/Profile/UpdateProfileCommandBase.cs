using JobBoardPlatform.BLL.Services.Session;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Commands.Profile
{
    public abstract class UpdateProfileCommandBase<TProfile, TData> : ICommand 
        where TProfile : class, IUserProfileEntity
        where TData : class
    {
        private readonly int id;
        private readonly TData data;
        private readonly IRepository<TProfile> repository;
        private readonly HttpContext httpContext;


        public UpdateProfileCommandBase(int profileId, TData profileData, 
            IRepository<TProfile> repository, 
            HttpContext httpContext)
        {
            this.id = profileId;
            this.data = profileData;
            this.repository = repository;
            this.httpContext = httpContext;
        }

        public async Task Execute()
        {
            var profile = await repository.Get(id);

            var viewModel = data;

            // TODO: validate data here for stream size
            // and extension... and add a model error

            await UploadFiles(viewModel, profile);

            MapDataToModel(viewModel, profile);

            await repository.Update(profile);

            var userSession = new UserSessionService<TProfile>(httpContext);
            await userSession.UpdateSessionStateAsync(profile);
        }

        protected abstract Task UploadFiles(TData from, TProfile to);

        protected abstract void MapDataToModel(TData from, TProfile to);
    }
}
