
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;

namespace JobBoardPlatform.DAL.Repositories.Blob.Settings
{
    public class BlobStorageSettings : IBlobStorageSettings
    {
        public string GetContainerName(Type storageType)
        {
            if (storageType == typeof(UserProfileImagesStorage))
            {
                return "userprofileimagescontainer";
            }
            else if (storageType == typeof(UserProfileAttachedResumeStorage))
            {
                return "userprofileattachedresumecontainer";
            }
            else if (storageType == typeof(UserApplicationsResumeStorage))
            {
                return "userapplicationsresumecontainer";
            }
            throw new NotImplementedException();
        }
    }
}
