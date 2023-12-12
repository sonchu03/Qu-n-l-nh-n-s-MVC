namespace WebQLNS.Models
{
    public class Users
    {
        public int Id { get; set; }
        public int RoleId { get; set; } 

        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public Roles Role { get; set; }
    }
}
