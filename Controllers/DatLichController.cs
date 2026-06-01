using Doantotnghiep.Models.Entities;
using Doantotnghiep.Models.Enum;
using Doantotnghiep.Models.ViewModel;
using Doantotnghiep.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace Doantotnghiep.Controllers
{
    public class DatLichController : Controller
    {
        private readonly IDatLichServices _datLichServices;
        private readonly IDichVuServices _dichVuServices;
        private readonly IChiTietDatLichServices _chiTietDatLichServices;
        private readonly IHoaDonService _hoaDonService;
        public DatLichController(IDatLichServices datLichServices,IChiTietDatLichServices chiTietDatLichServices, IDichVuServices dichVuServices, IHoaDonService hoaDonService)
        {
            _datLichServices = datLichServices;
            _dichVuServices = dichVuServices;
            _chiTietDatLichServices = chiTietDatLichServices;
            _hoaDonService = hoaDonService;
        }
        [HttpGet]
        [Authorize(Roles = "Admin,NhanVien,KhachHang")]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var datLichs = await _datLichServices.GetAllDatLichAsync();
                return View(datLichs);
            }

            if (User.IsInRole("NhanVien"))
            {
                var nhanVienIdClaim = User.FindFirst("IdNhanVien")?.Value;

                if (string.IsNullOrEmpty(nhanVienIdClaim))
                {
                    TempData["ErrorMessage"] = "Tài khoản nhân viên chưa có IdNhanVien trong claim.";
                    return Forbid();
                }

                var nhanVienId = int.Parse(nhanVienIdClaim);

                var datLichs = await _datLichServices.GetDatLichByNhanVienIdAsync(nhanVienId);
                return View(datLichs);
            }

            if (User.IsInRole("KhachHang"))
            {
                var khachHangIdClaim = User.FindFirst("IdKhachHang")?.Value;

                if (string.IsNullOrEmpty(khachHangIdClaim))
                {
                    TempData["ErrorMessage"] = "Tài khoản khách hàng chưa có IdKhachHang trong claim.";
                    return Forbid();
                }

                var khachHangId = int.Parse(khachHangIdClaim);

                var datLichs = await _datLichServices.GetDatLichByKhachHangIdAsync(khachHangId);
                return View(datLichs);
            }

            return Forbid();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var datLichs = await _datLichServices.GetDatLichByIdAsync(id);
            if (datLichs == null)
            {
                return NotFound();
            }
            if (User.IsInRole("Admin"))
            {
                return View(datLichs);
            }
            else if (User.IsInRole("NhanVien"))
            {
                var nhanVienIdClaim = User.FindFirst("IdNhanVien")?.Value;
                if (string.IsNullOrEmpty(nhanVienIdClaim))
                {
                    return Forbid();
                }

                var nhanVienId = int.Parse(nhanVienIdClaim);

                if (datLichs.IdNhanVien != nhanVienId)
                {
                    return Forbid();
                }
                
            }
            else if (User.IsInRole("KhachHang"))
            {
                var khachHangIdClaim = User.FindFirst("IdKhachHang")?.Value;
                if (string.IsNullOrEmpty(khachHangIdClaim))
                {
                    return Forbid();
                }

                var khachHangId = int.Parse(khachHangIdClaim);
                if (datLichs.IdKhachHang != khachHangId)
                {
                    return Forbid();
                }
            }

            var chiTietDichVus = await _chiTietDatLichServices.GetByDatLichIdAsync(id);
            var hoaDon = await _hoaDonService.GetByDatLichAsync(id);

            ViewBag.ChiTietDichVus = chiTietDichVus;
            ViewBag.HoaDon = hoaDon;

            return View(datLichs);
        }
        [HttpGet]
        [Authorize(Roles ="KhachHang")]
        public async Task<IActionResult> Create()
        {
            var model = new CreateDatLichViewModel
            {
                NgayHenLich = DateTime.Today
            };
            await LoadCreateData(model);
            return View(model);
        }
        private async Task LoadCreateData (CreateDatLichViewModel model)
        {
            var nhanViens = await _datLichServices.GetNhanVienSelectListAsync();
            //lay danh sach dich vu 
            var dichVus = await _datLichServices.GetDichVuSelectListAsync();
            //tao view model va gan danh sach vao 
            model.NhanViens = nhanViens.Select(nv => new SelectListItem
            {
                Value = nv.IdNhanVien.ToString(),
                Text = nv.TenNhanVien
            });
            model.DichVus = dichVus.Select(dv => new DichVuSelectItem
            {
                Value = dv.IdDichVu.ToString(),
                Text = dv.TenDichVu,
                ThoiLuongDV = dv.ThoiLuongDV
            });
            if(model.NgayHenLich == default)
            {
                model.NgayHenLich = DateTime.Today;  
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "KhachHang")]
        public async Task<IActionResult> Create(CreateDatLichViewModel datLich)
        {
            if (!ModelState.IsValid)
            {
                await LoadCreateData(datLich);
                return View(datLich);
            }
            try
            {
                var khachHangIdClaim = User.Claims.FirstOrDefault(c => c.Type == "IdKhachHang")?.Value;
                if (string.IsNullOrEmpty(khachHangIdClaim))
                {
                    return Forbid();
                }
                if(datLich.IdDichVu == null || !datLich.IdDichVu.Any())
                {
                    ModelState.AddModelError("IdDichVu", "Vui lòng chọn ít nhất một dịch vụ.");
                    await  LoadCreateData(datLich);
                    return View(datLich);
                }
                var danhsachDichVu = await _dichVuServices.GetDichVusByIdAsync(datLich.IdDichVu);
                if(danhsachDichVu == null || !danhsachDichVu.Any())
                {
                    ModelState.AddModelError("", "Danh sach dich vu khong hop le.");
                    await LoadCreateData(datLich);
                    return View(datLich);
                }
                if (danhsachDichVu.Count != datLich.IdDichVu.Count)
                {
                    ModelState.AddModelError("", "Có dịch vụ không tồn tại.");
                    await LoadCreateData(datLich);
                    return View(datLich);
                }
                DateTime giobatdau = datLich.NgayHenLich.Date.Add(datLich.GioBatDauDV);
                int tongthoigian = danhsachDichVu.Sum(dv => dv.ThoiLuongDV);
                DateTime gioketthuc = giobatdau.AddMinutes(tongthoigian);

                var datlich1 = new DatLich
                {
                    IdKhachHang = int.Parse(khachHangIdClaim),
                    IdNhanVien = datLich.IdNhanVien,
                    NgayHenLich = datLich.NgayHenLich.Date,
                    GioBatDauDV = giobatdau,
                    GioKetThucDV = gioketthuc,
                    GhiChuDatLich = datLich.GhiChuDatLich,
                    TrangThaiDatLich = TrangThaiDatLich.ChoXacNhan,
                    NgayTaoDatLich = DateTime.Now
                };
                await _datLichServices.CreateDatLichAsync(datlich1);
                foreach(var dichvu in danhsachDichVu)
                {
                    var chiTiet = new ChiTietDatLich
                    {
                        IdDatLich = datlich1.IdDatLich,
                        IdDichVu = dichvu.IdDichVu,
                        GiaDichVu = dichvu.GiaDichVu,
                        SoLuong = 1
                    };
                    await _chiTietDatLichServices.CreateAsync(chiTiet);
                }
                TempData["SuccessMessage"] = "Đặt lịch thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await LoadCreateData(datLich);
                return View(datLich);
            }
        }
        [Authorize(Roles = ("Admin,KhachHang"))]
        [HttpGet]
        public async Task<IActionResult> EditKhachHang(int id)
        {
            var datLich = await _datLichServices.GetDatLichByIdAsync(id);
            if (datLich == null)
            {
                return NotFound();
            }
            var khachHangIdClaim = User.Claims.FirstOrDefault(c => c.Type == "IdKhachHang")?.Value;
            if (string.IsNullOrEmpty(khachHangIdClaim))
            {
                return Forbid();
            }
            var khachHangId = int.Parse(khachHangIdClaim);
            //Khach hang chi duoc phep chinh sua dat lich cua minh
            if (datLich.IdKhachHang != khachHangId)
            {
                return Forbid();
            }
            var datLichViewModel = new KhachHangDatLichViewModel
            {
                IdDatLich = datLich.IdDatLich,
                NgayHenLich = datLich.NgayHenLich,
                GhiChuDatLich = datLich.GhiChuDatLich,
            };
            return View(datLichViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("Admin,KhachHang"))]
        public async Task<IActionResult> EditKhachHang(int id, KhachHangDatLichViewModel datLich)
        {
            if (!ModelState.IsValid)
            {
                return View(datLich);
            }
           
            var khachHangIdClaim = User.Claims.FirstOrDefault(c => c.Type == "IdKhachHang")?.Value;
            if (string.IsNullOrEmpty(khachHangIdClaim))
            {
                return Forbid();
            }
            var khachHangId = int.Parse(khachHangIdClaim);
            if (datLich.IdDatLich != id)
            {
                return NotFound();
            }
            try
            {
                var datLichEntity = await _datLichServices.GetDatLichByIdAsync(datLich.IdDatLich);
                if (datLichEntity == null)
                {
                    return NotFound();
                }
                //chi duoc phep chinh sua dat lich cua minh
                if (datLichEntity.IdKhachHang != khachHangId)
                {
                    return Forbid();
                }
                datLichEntity.NgayHenLich = datLich.NgayHenLich;
                datLichEntity.GhiChuDatLich = datLich.GhiChuDatLich;
                var result = await _datLichServices.UpdateDatLichAsync(datLichEntity);
                if (!result)
                {
                    TempData["ErrorMessage"] = "Cập nhật đặt lịch thất bại.";
                    return View(datLich);
                }
                TempData["SuccessMessage"] = "Cập nhật đặt lịch thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi cập nhật đặt lịch: {ex.Message}";
                return View(datLich);
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin,NhanVien")]
        public async Task<IActionResult> EditNhanVien(int id)
        {
            var datLich = await _datLichServices.GetDatLichByIdAsync(id);
            if (datLich == null)
            {
                return NotFound();
            }
            if (User.IsInRole("NhanVien"))
            {
                var nhanVienIdClaim = User.FindFirst("IdNhanVien")?.Value;
                if (string.IsNullOrEmpty(nhanVienIdClaim))
                {
                    return Forbid();
                }
                var nhanVienId = int.Parse(nhanVienIdClaim);
                //Nhan vien chi duoc phep chinh sua dat lich cua minh
                if (datLich.IdNhanVien != nhanVienId)
                {
                    return Forbid();
                }
            } 
            var datLichViewModel = new NhanVienDatLichViewModel
            {
                IdDatLich = datLich.IdDatLich,
                IdNhanVien = datLich.IdNhanVien,
                TrangThaiDatLich = datLich.TrangThaiDatLich,
                NgayHenLich = datLich.NgayHenLich,
                GhiChuDatLich = datLich.GhiChuDatLich,
                GioBatDauDV = datLich.GioBatDauDV,
                GioKetThucDV = datLich.GioKetThucDV
            };
            return View(datLichViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,NhanVien")]
        public async Task<IActionResult> EditNhanVien(int id, NhanVienDatLichViewModel datLich)
        {
            if (!ModelState.IsValid)
            {
                return View(datLich);
            }
            if(id != datLich.IdDatLich)
            {
                return NotFound();
            }
            if(User.IsInRole("NhanVien"))
            {
                var nhanVienIdClaim = User.FindFirst("IdNhanVien")?.Value;
                if (string.IsNullOrEmpty(nhanVienIdClaim))
                {
                    return Forbid();
                }
                var nhanVienId = int.Parse(nhanVienIdClaim);
                //Nhan vien chi duoc phep chinh sua dat lich cua minh
                if (datLich.IdNhanVien != nhanVienId)
                {
                    return Forbid();
                }
            }


            try
            {
                var datLichEntity = await _datLichServices.GetDatLichByIdAsync(datLich.IdDatLich);
                if (datLichEntity == null)
                {
                    return NotFound();
                }
               
                datLichEntity.TrangThaiDatLich = datLich.TrangThaiDatLich;
                datLichEntity.NgayHenLich = datLich.NgayHenLich;
                datLichEntity.GhiChuDatLich = datLich.GhiChuDatLich;
                datLichEntity.GioBatDauDV = datLich.GioBatDauDV;
                datLichEntity.GioKetThucDV = datLich.GioKetThucDV;
                var result = await _datLichServices.UpdateDatLichAsync(datLichEntity);
                if (!result)
                {
                    TempData["ErrorMessage"] = "Cập nhật đặt lịch thất bại.";
                    return View(datLich);
                }
                TempData["SuccessMessage"] = "Cập nhật đặt lịch thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi cập nhật đặt lịch: {ex.Message}";
                return View(datLich);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,NhanVien")]
        public async Task<IActionResult> UpdateTrangThai(int id, TrangThaiDatLich trangThai)
        {
            try
            {
                var result = await _datLichServices.UpdateTrangThaiAsync(id,trangThai);
                if (!result)
                {
                    TempData["ErrorMessage"] = "Cập nhật trạng thái đặt lịch thất bại.";
                    return NotFound();
                }
                TempData["SuccessMessage"] = "Cập nhật trạng thái đặt lịch thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi cập nhật trạng thái đặt lịch: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles =("Admin,NhanVien"))]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {               
                var trangthaihuy = await _datLichServices.UpdateTrangThaiAsync(id, TrangThaiDatLich.DaHuy);
                if (!trangthaihuy)
                {
                    TempData["ErrorMessage"] = "Hủy đặt lịch thất bại.";
                    return RedirectToAction(nameof(Index));
                }
                TempData["SuccessMessage"] = "Hủy đặt lịch thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi hủy đặt lịch: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
