using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebQLNS.Models
{
    public class ChuyenVien
    {
        [Key]
        public int Id { get; set; }
        public int MaChuyenVien { get; set; }
        public string TenChuyenVien { get; set; }
        public string ChuyenMon { get; set; }
        public int KinhNghiem { get; set; }

        [ForeignKey("PhongBan")]
        public int MaPhongBan { get; set; }
        public PhongBan PhongBan { get; set; }
    }
}
