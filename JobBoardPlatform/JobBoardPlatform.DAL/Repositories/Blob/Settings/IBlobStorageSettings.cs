
namespace JobBoardPlatform.DAL.Repositories.Blob.Settings
{
    public interface IBlobStorageSettings
    {
        string GetContainerName(Type storageType);
    }
}
