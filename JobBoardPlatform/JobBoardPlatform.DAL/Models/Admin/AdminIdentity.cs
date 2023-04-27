namespace JobBoardPlatform.DAL.Models.Admin
{
    public class AdminIdentity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string HashPassword { get; set; }
    }
}
