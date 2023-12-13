using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebQLNS.Models
{
    public class Reward
    {
        [Key]
        public int RewardId { get; set; }
        public int MaNhanVien { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

        [ForeignKey("MaNhanVien")]
        public NhanVien NhanVien { get; set; }
    }
}
