using Microsoft.EntityFrameworkCore;
using WebQLNS.Models;

namespace WebQLNS.Controllers
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        { 
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NhanVien>().HasOne(nv => nv.PhongBan).WithMany(pb => pb.NhanViens).HasForeignKey(nv => nv.MaPhongBan);

            modelBuilder.Entity<ChuyenVien>().HasOne(cv => cv.PhongBan).WithMany(pb => pb.ChuyenViens).HasForeignKey(cv => cv.MaPhongBan);

            modelBuilder.Entity<Users>().HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<PhongBan> PhongBans { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<ChuyenVien> ChuyenViens { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<Penalty> Penalty { get; set; }
    }
}