namespace JobBoardPlatform.BLL.Common.Contracts
{
    public interface IFactory<T>
    {
        T Create();
    }
}
