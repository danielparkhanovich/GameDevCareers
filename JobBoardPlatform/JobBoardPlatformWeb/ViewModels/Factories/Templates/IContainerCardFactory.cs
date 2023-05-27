using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.PL.ViewModels.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Factories.Templates
{
    public interface IContainerCardFactory<T> where T : IEntity
    {
        IContainerCard CreateCard(T entity);
    }
}
