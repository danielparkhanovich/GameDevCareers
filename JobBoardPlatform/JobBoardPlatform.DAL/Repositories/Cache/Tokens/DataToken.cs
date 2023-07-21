

namespace JobBoardPlatform.DAL.Repositories.Cache.Tokens
{
    public class DataToken<T> : IToken
    {
        public string Id { get; set; }
        public T Value { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
