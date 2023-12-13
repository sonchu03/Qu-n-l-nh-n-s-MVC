using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebQLNS.Models;

namespace WebQLNS.Controllers
{
    public class PhongBanController : Controller
    {
        private readonly AppDbContext _context;

        public PhongBanController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PhongBan
        public async Task<IActionResult> Index()
        {
            string username = HttpContext.Session.GetString("Username");
            string roleName = HttpContext.Session.GetString("RoleName");
            // Truyền tên tài khoản vào ViewBag
            ViewBag.Username = username;
            ViewBag.RoleName = roleName;
            return _context.PhongBans != null ? 
                          View(await _context.PhongBans.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.PhongBans'  is null.");
        }

        // GET: PhongBan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PhongBans == null)
            {
                return NotFound();
            }

            var phongBan = await _context.PhongBans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (phongBan == null)
            {
                return NotFound();
            }

            return View(phongBan);
        }
        public async Task<IActionResult> Test(int? id)
        {
            if (id == null || _context.PhongBans == null)
            {
                return NotFound();
            }

            var phongBan = await _context.PhongBans
                .Include(pb => pb.NhanViens)
                .Include(pb => pb.ChuyenViens)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (phongBan == null)
            {
                return NotFound();
            }

            var chuyenViens = phongBan.ChuyenViens.ToList();

            ViewBag.ChuyenViens = chuyenViens;

            return View(phongBan);
        }

        // GET: PhongBan/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PhongBan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MaPhongBan,TenPhongBan,DiaChi,DienThoai")] PhongBan phongBan)
        {

                _context.Add(phongBan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            return View(phongBan);
        }

        // GET: PhongBan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PhongBans == null)
            {
                return NotFound();
            }

            var phongBan = await _context.PhongBans.FindAsync(id);
            if (phongBan == null)
            {
                return NotFound();
            }
            return View(phongBan);
        }

        // POST: PhongBan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MaPhongBan,TenPhongBan,DiaChi,DienThoai")] PhongBan phongBan)
        {
            if (id != phongBan.Id)
            {
                return NotFound();
            }


                try
                {
                    _context.Update(phongBan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhongBanExists(phongBan.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                return RedirectToAction(nameof(Index));
            }
            return View(phongBan);
        }

        // GET: PhongBan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PhongBans == null)
            {
                return NotFound();
            }

            var phongBan = await _context.PhongBans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (phongBan == null)
            {
                return NotFound();
            }

            return View(phongBan);
        }

        // POST: PhongBan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PhongBans == null)
            {
                return Problem("Entity set 'AppDbContext.PhongBans'  is null.");
            }
            var phongBan = await _context.PhongBans.FindAsync(id);
            if (phongBan != null)
            {
                _context.PhongBans.Remove(phongBan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhongBanExists(int id)
        {
          return (_context.PhongBans?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
