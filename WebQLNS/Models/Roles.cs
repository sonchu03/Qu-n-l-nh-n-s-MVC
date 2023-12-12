namespace WebQLNS.Models
{
    public class Roles
    {
        public int Id { get; set; }
        public string RoleName { get; set; }

        // Khai báo thuộc tính điều hướng
        public ICollection<Users> Users { get; set; }
    }
}
