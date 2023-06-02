
using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Query
{
    internal interface IQuery<T> where T : IEntity
    {
        Task<T> Get();
    }
}
