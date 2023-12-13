using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebQLNS.Models;

namespace WebQLNS.Controllers
{
    public class NhanVienController : Controller
    {
        private readonly AppDbContext _context;

        public NhanVienController(AppDbContext context)
        {
            _context = context;
        }

        // GET: NhanVien
        public async Task<IActionResult> Index()
        {
            string username = HttpContext.Session.GetString("Username");
            string roleName = HttpContext.Session.GetString("RoleName");
            // Truyền tên tài khoản vào ViewBag
            ViewBag.Username = username;
            ViewBag.RoleName = roleName;
            var appDbContext = _context.NhanViens.Include(n => n.PhongBan);
            return View(await appDbContext.ToListAsync());
        }

        // GET: NhanVien/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NhanViens == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens
                .Include(n => n.PhongBan)
                .FirstOrDefaultAsync(m => m.MaNhanVien == id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(nhanVien);
        }

        // GET: NhanVien/Create
        public IActionResult Create()
        {
            ViewData["MaPhongBan"] = new SelectList(_context.PhongBans, "Id", "Id");
            return View();
        }

        // POST: NhanVien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaNhanVien,TenNhanVien,NgaySinh,GioiTinh,Luong,MaPhongBan")] NhanVien nhanVien)
        {

                _context.Add(nhanVien);
                await _context.SaveChangesAsync();

                // Tạo đối tượng user từ thông tin nhân viên
                Users user = new Users
                {
                    UserId = nhanVien.MaNhanVien,
                    Username = GenerateUsername(nhanVien.TenNhanVien),
                    Password = GeneratePassword(),
                    RoleId = 2,
                    // Các thông tin khác
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            ViewData["MaPhongBan"] = new SelectList(_context.PhongBans, "Id", "Id", nhanVien.MaPhongBan);
            return View(nhanVien);
        }
        private string GeneratePassword()
        {
            string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder passwordBuilder = new StringBuilder();
            Random random = new Random();

            // Tạo mật khẩu ngẫu nhiên với độ dài mong muốn (ví dụ: 8 ký tự)
            int passwordLength = 8;

            for (int i = 0; i < passwordLength; i++)
            {
                int randomIndex = random.Next(validChars.Length);
                passwordBuilder.Append(validChars[randomIndex]);
            }

            return passwordBuilder.ToString();
        }
        private string GenerateUsername(string tenNhanVien)
        {
            // Xử lý để tạo username từ tên nhân viên, ví dụ:
            string[] nameParts = tenNhanVien.ToLower().Split(' ');
            string firstName = nameParts[0];
            string lastName = nameParts[nameParts.Length - 1];
            string username = $"{firstName}.{lastName}";

            return username;
        }

        // GET: NhanVien/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NhanViens == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            ViewData["MaPhongBan"] = new SelectList(_context.PhongBans, "Id", "Id", nhanVien.MaPhongBan);
            return View(nhanVien);
        }

        // POST: NhanVien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaNhanVien,TenNhanVien,NgaySinh,GioiTinh,Luong,MaPhongBan")] NhanVien nhanVien)
        {
            if (id != nhanVien.MaNhanVien)
            {
                return NotFound();
            }


                try
                {
                    _context.Update(nhanVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhanVienExists(nhanVien.MaNhanVien))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            ViewData["MaPhongBan"] = new SelectList(_context.PhongBans, "Id", "Id", nhanVien.MaPhongBan);
            return View(nhanVien);
        }

        // GET: NhanVien/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NhanViens == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens
                .Include(n => n.PhongBan)
                .FirstOrDefaultAsync(m => m.MaNhanVien == id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(nhanVien);
        }

        // POST: NhanVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NhanViens == null)
            {
                return Problem("Entity set 'AppDbContext.NhanViens'  is null.");
            }
            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien != null)
            {
                _context.NhanViens.Remove(nhanVien);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult SalesReports()
        {
            string username = HttpContext.Session.GetString("Username");
            string roleName = HttpContext.Session.GetString("RoleName");
            // Truyền tên tài khoản vào ViewBag
            ViewBag.Username = username;
            ViewBag.RoleName = roleName;
            var nhanvien = _context.NhanViens.ToList();
            foreach (var nhanVien in nhanvien)
            {
                // Lấy thông tin các phần thưởng của nhân viên
                decimal totalRewards = _context.Rewards
                    .Where(r => r.MaNhanVien == nhanVien.MaNhanVien)
                    .Sum(r => r.Amount);

                // Lấy thông tin các khoản phạt của nhân viên
                decimal totalPenalties = _context.Penalty
                    .Where(p => p.MaNhanVien == nhanVien.MaNhanVien)
                    .Sum(p => p.Amount);

                // Tính toán lương
                decimal salary = (nhanVien.Luong ?? 0) + totalRewards - totalPenalties;

                // Lưu lương vào biến employee hoặc tạo một DTO (Data Transfer Object) để lưu thông tin lương
                nhanVien.Luong = (int)salary;
            }
            return View(nhanvien);

        }
        private bool NhanVienExists(int id)
        {
          return (_context.NhanViens?.Any(e => e.MaNhanVien == id)).GetValueOrDefault();
        }
    }
}
