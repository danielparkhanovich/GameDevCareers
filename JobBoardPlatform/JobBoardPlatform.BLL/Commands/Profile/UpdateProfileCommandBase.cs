using JobBoardPlatform.BLL.Services.Session;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Commands.Profile
{
    public abstract class UpdateProfileCommandBase<TEntity, TProfile, TData> : ICommand 
        where TEntity : class, IUserIdentityEntity
        where TProfile : class, IUserProfileEntity
        where TData : class
    {
        private readonly int id;
        private readonly TData data;
        private readonly IRepository<TProfile> repository;


        public UpdateProfileCommandBase(
            int profileId, 
            TData profileData, 
            IRepository<TProfile> repository)
        {
            this.id = profileId;
            this.data = profileData;
            this.repository = repository;
        }

        public async Task Execute()
        {
            var profile = await repository.Get(id);

            var viewModel = data;

            await UploadFiles(viewModel, profile);
            MapDataToModel(viewModel, profile);
            await repository.Update(profile);
        }

        protected abstract Task UploadFiles(TData from, TProfile to);

        protected abstract void MapDataToModel(TData from, TProfile to);
    }
}
