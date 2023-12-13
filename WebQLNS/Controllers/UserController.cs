using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebQLNS.Models;
using Microsoft.AspNetCore.Http;

namespace WebQLNS.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public UserController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.Include(u => u.Role).ToListAsync();

            ViewData["Users"] = users;
            string username = HttpContext.Session.GetString("Username");
            string roleName = HttpContext.Session.GetString("RoleName");
            // Truyền tên tài khoản vào ViewBag
            ViewBag.Username = username;
            ViewBag.RoleName = roleName;

            return View();
        }
        public ActionResult Login()
        {
            return PartialView("_LoginPartial");
        }

        // POST: /Users/Login
        [HttpPost]
        public IActionResult Login(Users user)
        {
            var dbUser = _context.Users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
            if (dbUser != null)
            {
                // Đăng nhập thành công

                // Lấy thông tin quyền từ cơ sở dữ liệu
                var userWithRole = _context.Users
                    .Where(u => u.UserId == dbUser.UserId)
                    .Join(
                        _context.Roles,
                        user => user.RoleId,
                        role => role.Id,
                        (user, role) => new { User = user, Role = role }
                    )
                    .FirstOrDefault();

                if (userWithRole != null && userWithRole.User != null && userWithRole.Role != null)
                {
                    int userId = userWithRole.User.UserId;
                    int roleId = userWithRole.Role.Id;
                    string roleName = userWithRole.Role.RoleName;

                    // Lưu thông tin người dùng và quyền vào session
                    HttpContext.Session.SetInt32("UserId", userId);
                    HttpContext.Session.SetString("Username", dbUser.Username);
                    HttpContext.Session.SetInt32("RoleId", roleId);
                    HttpContext.Session.SetString("RoleName", roleName);
                    CheckIn(userId);

                    return RedirectToAction("Index", "Home");
                }
            }

            // Đăng nhập thất bại, hiển thị thông báo lỗi
            ModelState.AddModelError("", "Invalid username or password");
            return PartialView("_LoginPartial", user);
        }
        private void CheckIn(int userId)
        {
            // Kiểm tra xem người dùng có tồn tại trong hệ thống hay không
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return;
            }

            // Tạo một đối tượng chấm công mới và thiết lập các thuộc tính
            var attendance = new Attendance
            {
                UserId = userId,
                CheckInTime = DateTime.Now,
                CheckOutTime = DateTime.MinValue
            };

            // Lưu trữ đối tượng chấm công vào cơ sở dữ liệu
            SaveAttendance(attendance);
        }

        private void SaveAttendance(Attendance attendance)
        {
            // Lưu trữ đối tượng chấm công vào cơ sở dữ liệu
            _context.Attendances.Add(attendance);
            _context.SaveChanges();
        }
        public IActionResult Logout()
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            // Xóa các giá trị session liên quan đến người dùng
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("RoleId");
            HttpContext.Session.Remove("RoleName");

            if (userId != 0)
            {
                CheckOut(userId);
            }
            // Chuyển hướng đến trang đăng nhập
            return RedirectToAction("Login", "User");
        }
        private void CheckOut(int userId)
        {
            // Kiểm tra xem người dùng có tồn tại trong hệ thống hay không
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return;
            }

            // Tìm kiếm đối tượng chấm công cuối cùng chưa có giờ CheckOut
            var attendance = _context.Attendances
                .Where(a => a.UserId == userId && a.CheckOutTime == DateTime.MinValue)
                .OrderByDescending(a => a.CheckInTime)
                .FirstOrDefault();

            if (attendance != null)
            {
                // Cập nhật giờ CheckOut thành thời gian hiện tại
                attendance.CheckOutTime = DateTime.Now;
                _context.SaveChanges();
            }
        }
        private string GetUserNameById(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            return user != null ? user.Username : string.Empty;
        }
        public IActionResult GetAllAttendance()
        {
            string username = HttpContext.Session.GetString("Username");
            string roleName = HttpContext.Session.GetString("RoleName");
            // Truyền tên tài khoản vào ViewBag
            ViewBag.Username = username;
            ViewBag.RoleName = roleName;
            var allAttendances = _context.Attendances.ToList();
            var userIds = allAttendances.Select(a => a.UserId).Distinct().ToList();
            var users = _context.Users.Where(u => userIds.Contains(u.UserId)).ToList();

            ViewBag.AllAttendances = allAttendances;
            ViewBag.Users = users;

            return View();
        }
        public IActionResult DeleteAttendance(int id)
        {
            // Tìm dữ liệu chấm công theo id trong nguồn dữ liệu của bạn
            var attendance = _context.Attendances.FirstOrDefault(a => a.Id == id);

            // Kiểm tra xem dữ liệu chấm công có tồn tại không
            if (attendance == null)
            {
                // Trả về trạng thái lỗi hoặc thông báo tương ứng
                return NotFound();
            }

            // Xóa dữ liệu chấm công từ nguồn dữ liệu của bạn
            _context.Attendances.Remove(attendance);
            _context.SaveChanges();

            // Thực hiện các thao tác tiếp theo, ví dụ: chuyển hướng đến một trang khác hoặc làm mới trang hiện tại
            return RedirectToAction("GetAllAttendance");
        }
        //về phân quyền nhứo add vào
        [HttpPost]
        public IActionResult KiemTra()
        {
            var roleName = HttpContext.Session.GetString("RoleName");
            if (!string.IsNullOrEmpty(roleName))
            {
                if (roleName == "Admin")
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (roleName == "NhanVien")
                {
                    return RedirectToAction("Index", "NhanVien");
                }
                else if (roleName == "ChuyenVien")
                {
                    return RedirectToAction("Index", "ChuyenVien");
                }
            }

            return View();
        }
        public IActionResult Profile()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                // Lấy thông tin từ session
                int userId = HttpContext.Session.GetInt32("UserId").Value;

                // Lấy thông tin người dùng từ cơ sở dữ liệu
                var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
                var nhanVien = _context.NhanViens
                    .Include(nv => nv.PhongBan)  // Eager loading thông tin PhongBan
                    .FirstOrDefault(nv => nv.MaNhanVien == userId);

                if (user != null && nhanVien != null)
                {
                    // Tạo đối tượng ViewModel
                    var viewModel = new ProfileViewModel
                    {
                        User = user,
                        NhanVien = nhanVien
                    };

                    // Truyền thông tin ViewModel đến view
                    return View(viewModel);
                }
            }
            return RedirectToAction("Index", "Home");
        }

            public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }


        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "RoleName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoleId,UserId,Username,Password")] Users users)
        {
                _context.Add(users);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "RoleName", users.RoleId);
            return View(users);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", users.RoleId);
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoleId,UserId,Username,Password")] Users users)
        {
            if (id != users.Id)
            {
                return NotFound();
            }
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", users.RoleId);
            return View(users);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'AppDbContext.Users'  is null.");
            }
            var users = await _context.Users.FindAsync(id);
            if (users != null)
            {
                _context.Users.Remove(users);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
