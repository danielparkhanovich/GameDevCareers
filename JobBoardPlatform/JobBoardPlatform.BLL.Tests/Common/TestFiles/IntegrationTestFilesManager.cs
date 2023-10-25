using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.IntegrationTests.Common.TestFiles
{
    public static class IntegrationTestFilesManager
    {
        public static IFormFile GetExampleResumeFile()
        {
            string directoryPath = Directory.GetCurrentDirectory();
            return GetFileAsFormFile(Path.Combine(directoryPath, $"{GetFilesPathPrefix()}/Resumes/resume0.pdf"), "application/pdf");
        }

        public static IFormFile GetEmployeeProfileImageFile()
        {
            string directoryPath = Directory.GetCurrentDirectory();
            return GetFileAsFormFile(Path.Combine(directoryPath, $"{GetFilesPathPrefix()}/Images/userProfileImage0.png"), "image/bitmap");
        }

        public static IFormFile GetCompanyProfileImageFile()
        {
            string directoryPath = Directory.GetCurrentDirectory();
            return GetFileAsFormFile(Path.Combine(directoryPath, $"{GetFilesPathPrefix()}/Images/companyLogo0.jpg"), "image/bitmap");
        }

        private static string GetFilesPathPrefix()
        {
            return "../../../Common/TestFiles";
        }

        private static IFormFile GetFileAsFormFile(string filePath, string contentType)
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                memoryStream.Position = 0;

                var formFile = new FormFile(memoryStream, 0, memoryStream.Length, "file", Path.GetFileName(stream.Name)) 
                {
                    Headers = new HeaderDictionary(),
                    ContentType = contentType
                };
                return formFile;
            }
        }
    }
}
