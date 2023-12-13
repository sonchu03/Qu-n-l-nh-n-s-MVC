using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebQLNS.Models
{
    public class NhanVien
    {
        [Key]
        public int MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public int? Luong { get; set; }

        [ForeignKey("PhongBan")]
        public int MaPhongBan { get; set; }
        public PhongBan PhongBan { get; set; }

        public ICollection<Reward> Rewards { get; set; }
        public ICollection<Penalty> Penalties { get; set; }

    }
}
