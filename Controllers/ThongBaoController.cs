using Doantotnghiep.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Doantotnghiep.Controllers
{
    [Authorize]
    public class ThongBaoController : Controller
    {
        private readonly AppDbContext _context;

        public ThongBaoController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var idKhachHangClaim = User.FindFirst("IdKhachHang")?.Value;

            if (!int.TryParse(idKhachHangClaim, out var idKhachHang))
            {
                return Forbid();
            }

            var data = await _context.ThongBaos
                .Where(x => x.IdKhachHang == idKhachHang)
                .OrderByDescending(x => x.NgayTao)
                .ToListAsync();

            return View(data);
        }

        public async Task<IActionResult> DocThongBao(int id)
        {
            var idKhachHangClaim = User.FindFirst("IdKhachHang")?.Value;

            if (!int.TryParse(idKhachHangClaim, out var idKhachHang))
            {
                return Forbid();
            }

            var thongBao = await _context.ThongBaos
                .FirstOrDefaultAsync(x => x.IdThongBao == id && x.IdKhachHang == idKhachHang);

            if (thongBao == null)
            {
                return NotFound();
            }

            thongBao.DaDoc = true;
            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(thongBao.DuongDan))
            {
                return Redirect(thongBao.DuongDan);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}