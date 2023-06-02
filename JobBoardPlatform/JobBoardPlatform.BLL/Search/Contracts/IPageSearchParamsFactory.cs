namespace JobBoardPlatform.BLL.Search.Contracts
{
    public interface IPageSearchParamsFactory<T> where T : class, IPageSearchParams, new()
    {
        public T GetSearchParams();
    }
}
