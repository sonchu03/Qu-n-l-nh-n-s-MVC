using System.ComponentModel.DataAnnotations;

namespace WebQLNS.Models
{
    public class PhongBan
    {
        [Key]
        public int Id { get; set; }
        public int MaPhongBan { get; set; }
        public string TenPhongBan { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public ICollection<ChuyenVien> ChuyenViens { get; set; }
        public ICollection<NhanVien> NhanViens { get; set; }
    }
}
