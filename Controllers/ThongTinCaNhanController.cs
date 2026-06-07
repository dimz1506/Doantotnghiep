using Doantotnghiep.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Doantotnghiep.Controllers
{
    public class ThongTinCaNhanController : Controller
    {
        private readonly AppDbContext _context;

        public ThongTinCaNhanController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("DangNhap", "Auth");
            }

            var tenTaiKhoan = User.Identity.Name;

            var taiKhoan = await _context.TaiKhoans
                .FirstOrDefaultAsync(x => x.TenTaiKhoan == tenTaiKhoan);

            if (taiKhoan == null)
            {
                return NotFound("Không tìm thấy tài khoản.");
            }

            if (User.IsInRole("NhanVien"))
            {
                var nhanVien = await _context.NhanViens
                    .Include(x => x.TaiKhoan)
                    .FirstOrDefaultAsync(x => x.IdTaiKhoan == taiKhoan.IdTaiKhoan);

                if (nhanVien == null)
                {
                    return NotFound("Không tìm thấy thông tin nhân viên.");
                }

                ViewBag.LoaiTaiKhoan = "NhanVien";
                return View(nhanVien);
            }

            if (User.IsInRole("KhachHang"))
            {
                var khachHang = await _context.KhachHangs
                    .Include(x => x.TaiKhoan)
                    .FirstOrDefaultAsync(x => x.IdTaiKhoan == taiKhoan.IdTaiKhoan);

                if (khachHang == null)
                {
                    return NotFound("Không tìm thấy thông tin khách hàng.");
                }

                ViewBag.LoaiTaiKhoan = "KhachHang";
                return View(khachHang);
            }

            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }

            return View();
        }
    }
}