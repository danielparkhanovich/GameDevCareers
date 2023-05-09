namespace JobBoardPlatform.BLL.Common.Formatter
{
    public interface ITextFormatter<T>
    {
        public string GetString(T value);
    }
}
