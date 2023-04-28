namespace JobBoardPlatform.BLL.Commands.Mappers
{
    /// <summary>
    /// Maps values from T to V
    /// </summary>
    /// <typeparam name="T">First object</typeparam>
    /// <typeparam name="V">Second object</typeparam>
    public interface IMapper<T, V>
    {
        void Map(T from, V to);
    }
}
