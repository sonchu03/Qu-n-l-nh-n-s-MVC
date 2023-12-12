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
    public class ChuyenVienController : Controller
    {
        private readonly AppDbContext _context;

        public ChuyenVienController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ChuyenVien
        public async Task<IActionResult> Index()
        {
            return View();
            var appDbContext = _context.ChuyenViens.Include(c => c.PhongBan);
            return View(await appDbContext.ToListAsync());
        }

        // GET: ChuyenVien/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ChuyenViens == null)
            {
                return NotFound();
            }

            var chuyenVien = await _context.ChuyenViens
                .Include(c => c.PhongBan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chuyenVien == null)
            {
                return NotFound();
            }

            return View(chuyenVien);
        }

        // GET: ChuyenVien/Create
        public IActionResult Create()
        {
            ViewData["MaPhongBan"] = new SelectList(_context.PhongBans, "Id", "Id");
            return View();
        }

        // POST: ChuyenVien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MaChuyenVien,TenChuyenVien,ChuyenMon,KinhNghiem,MaPhongBan")] ChuyenVien chuyenVien)
        {

                _context.Add(chuyenVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            ViewData["MaPhongBan"] = new SelectList(_context.PhongBans, "Id", "Id", chuyenVien.MaPhongBan);
            return View(chuyenVien);
        }

        // GET: ChuyenVien/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ChuyenViens == null)
            {
                return NotFound();
            }

            var chuyenVien = await _context.ChuyenViens.FindAsync(id);
            if (chuyenVien == null)
            {
                return NotFound();
            }
            ViewData["MaPhongBan"] = new SelectList(_context.PhongBans, "Id", "Id", chuyenVien.MaPhongBan);
            return View(chuyenVien);
        }

        // POST: ChuyenVien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MaChuyenVien,TenChuyenVien,ChuyenMon,KinhNghiem,MaPhongBan")] ChuyenVien chuyenVien)
        {
            if (id != chuyenVien.Id)
            {
                return NotFound();
            }


                try
                {
                    _context.Update(chuyenVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChuyenVienExists(chuyenVien.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            ViewData["MaPhongBan"] = new SelectList(_context.PhongBans, "Id", "Id", chuyenVien.MaPhongBan);
            return View(chuyenVien);
        }

        // GET: ChuyenVien/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ChuyenViens == null)
            {
                return NotFound();
            }

            var chuyenVien = await _context.ChuyenViens
                .Include(c => c.PhongBan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chuyenVien == null)
            {
                return NotFound();
            }

            return View(chuyenVien);
        }

        // POST: ChuyenVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ChuyenViens == null)
            {
                return Problem("Entity set 'AppDbContext.ChuyenViens'  is null.");
            }
            var chuyenVien = await _context.ChuyenViens.FindAsync(id);
            if (chuyenVien != null)
            {
                _context.ChuyenViens.Remove(chuyenVien);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChuyenVienExists(int id)
        {
          return (_context.ChuyenViens?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
