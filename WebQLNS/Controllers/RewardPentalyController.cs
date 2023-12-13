using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQLNS.Models;

namespace WebQLNS.Controllers
{
    public class RewardPentalyController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        public RewardPentalyController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            string username = HttpContext.Session.GetString("Username");
            string roleName = HttpContext.Session.GetString("RoleName");
            // Truyền tên tài khoản vào ViewBag
            ViewBag.Username = username;
            ViewBag.RoleName = roleName;
            var employees = await _context.NhanViens.Include(n => n.Rewards).Include(n => n.Penalties).ToListAsync();
            return View(employees);
        }
        [HttpGet]
        public IActionResult AddReward(int MaNhanVien)
        {
            ViewBag.MaNhanVien = MaNhanVien;
            return View();
        }

        [HttpPost]
        public IActionResult AddReward(Reward reward)
        {// Gán giá trị từ ViewBag.NhanVienId vào reward.MaNhanVien
            _context.Rewards.Add(reward);
            _context.SaveChanges();
            return RedirectToAction("Index", "RewardPentaly");
        }

        [HttpGet]
        public IActionResult AddPenalty(int MaNhanVien)
        {
            ViewBag.MaNhanVien = MaNhanVien;
            return View();
        }

        [HttpPost]
        public IActionResult AddPenalty(Penalty penalty)
        {
                _context.Penalty.Add(penalty);
                _context.SaveChanges();
                 return RedirectToAction("Index", "RewardPentaly");  

        }
    }
}
