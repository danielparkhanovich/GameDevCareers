using JobBoardPlatform.BLL.Boundaries;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.PL.ViewModels.Models.Authentification
{
    public class UserLoginViewModel : IUserLoginData
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
