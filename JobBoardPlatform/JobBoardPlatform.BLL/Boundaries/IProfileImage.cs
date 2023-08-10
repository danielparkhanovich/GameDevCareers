using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace JobBoardPlatform.BLL.Boundaries
{
    public interface IProfileImage
    {
        [JsonIgnore]
        public IFormFile? File { get; set; }
        public string? ImageUrl { get; set; }
    }
}
