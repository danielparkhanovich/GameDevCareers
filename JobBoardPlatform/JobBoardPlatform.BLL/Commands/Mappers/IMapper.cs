namespace JobBoardPlatform.BLL.Commands.Mappers
{
    /// <summary>
    /// Maps values from T to V
    /// </summary>
    /// <typeparam name="T">Map from object</typeparam>
    /// <typeparam name="V">Map to object</typeparam>
    public interface IMapper<T, V>
    {
        void Map(T from, V to);
    }
}
