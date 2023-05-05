namespace JobBoardPlatform.BLL.Search
{
    public interface ISearcher<T>
    {
        int AfterFiltersCount { get; set; }
        Task<T> Search();
    }
}
