namespace JobBoardPlatform.BLL.Models.Contracts
{
    public interface IUserLoginData
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
