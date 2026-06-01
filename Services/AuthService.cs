using Doantotnghiep.Data;
using Doantotnghiep.Helpers;
using Doantotnghiep.Models.Entities;
using Doantotnghiep.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Doantotnghiep.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        public AuthService(AppDbContext context)
        {
            _context = context;
        }
        // dang ky tai khoan
        public async Task<(bool Success, string Message)> RegisterCustomerAsync(RegisterCustomerVM model)
        {
            // kiem tra ten dang nhap da ton tai chua
            var existingUser = await _context.TaiKhoans
                .AnyAsync(tk => tk.TenTaiKhoan == model.TenDangNhap || tk.Email == model.Email);
            if (existingUser)
            {
                return (false, "Tên đăng nhập hoặc email đã tồn tại.");
            }
            //tao salt va hash password
            var salt = Argon2PasswordHelper.GenerateSalt();
            var hashedPassword = await Argon2PasswordHelper.HashPasswordAsync(model.MatKhau, salt);
            var user = new TaiKhoan
            {
                TenTaiKhoan = model.TenDangNhap,
                Passwordhash =hashedPassword,
                Salt = salt,
                IdVaiTro = 3, // mac dinh la khach hang
                Email = model.Email,
                SoDienThoai = model.SoDienThoai,
                TrangThaiTK = true
            };
            _context.TaiKhoans.Add(user);
            await _context.SaveChangesAsync();
            var KhachHang = new KhachHang
            {
                IdTaiKhoan = user.IdTaiKhoan,
                TenKhachHang = model.TenKhachHang,
                DiaChiKhachHang = model.DiaChiKhachHang,
                GhiChuKH = string.Empty
            };
            _context.KhachHangs.Add(KhachHang);
            await _context.SaveChangesAsync();
            return (true, "Đăng ký thành công. Bạn có thể đăng nhập ngay bây giờ.");
        }

        //Admin tao nhan vien
        public async Task<(bool Success, string Message)> CreateNhanVienAsync(CreateNhanVienVM createNhanVienVM)
        {
            var existingUser = await _context.TaiKhoans
                .AnyAsync(tk => tk.TenTaiKhoan == createNhanVienVM.TenDangNhap || tk.Email == createNhanVienVM.Email);
            if (existingUser)
            {
                return (false, "Tên đăng nhập hoặc email đã tồn tại.");
            }
            var salt = Argon2PasswordHelper.GenerateSalt();
            var hashedPassword = await Argon2PasswordHelper.HashPasswordAsync(createNhanVienVM.MatKhau, salt);
            var user = new TaiKhoan
            {
                TenTaiKhoan = createNhanVienVM.TenDangNhap,
                Passwordhash = hashedPassword,
                Salt = salt,
                IdVaiTro = 2, // nhan vien
                Email = createNhanVienVM.Email,
                SoDienThoai = createNhanVienVM.SoDienThoai,
                TrangThaiTK = true
            };
            _context.TaiKhoans.Add(user);
            await _context.SaveChangesAsync();
            var nhanvien = new NhanVien
            {
                IdTaiKhoan = user.IdTaiKhoan,
                TenNhanVien = createNhanVienVM.TenNhanVien,
                DiaChiNV = createNhanVienVM.DiaChiNhanVien,
                ChuyenMonNV = createNhanVienVM.ChuyenMonNV,
                TrangThaiNV = Doantotnghiep.Models.Enum.TrangThaiNhanVien.DangLamViec
            };
            _context.NhanViens.Add(nhanvien);
            await _context.SaveChangesAsync();
            return (true, "Tạo nhân viên thành công.");
        }
        //Dang nhap
        public async Task<LoginResultVM> LoginAsync(LoginVM model)
        {
            var user = await _context.TaiKhoans
                .FirstOrDefaultAsync(tk => tk.TenTaiKhoan == model.TenDangNhap);
            if (user == null || !user.TrangThaiTK)
            {
                return null;
            }
            var isValid = await Argon2PasswordHelper.VerifyPasswordAsync(
                model.MatKhau,
                user.Salt,
                user.Passwordhash);
            if(!isValid)
            {
                return null;
            }
            var result = new LoginResultVM
            {
                IdTaiKhoan = user.IdTaiKhoan,
                TenTaiKhoan = user.TenTaiKhoan,
                IdVaiTro = user.IdVaiTro
            };
            if(user.IdVaiTro == 2)
            {
                var nhanvien = await _context.NhanViens
                    .FirstOrDefaultAsync(nv => nv.IdTaiKhoan == user.IdTaiKhoan);
                if (nhanvien != null)
                {
                    result.IdNhanVien = nhanvien.IdNhanVien;
                }
            }
            else if (user.IdVaiTro == 3)
            {
                var khachhang = await _context.KhachHangs
                    .FirstOrDefaultAsync(kh => kh.IdTaiKhoan == user.IdTaiKhoan);
                if (khachhang != null)
                {
                    result.IdKhachHang = khachhang.IdKhachHang;
                }
            }
            return result;
        }
    }
}
