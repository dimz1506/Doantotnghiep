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
        public DatLichController(IDatLichServices datLichServices, IChiTietDatLichServices chiTietDatLichServices, IDichVuServices dichVuServices, IHoaDonService hoaDonService)
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
        [Authorize(Roles = "Admin,NhanVien,KhachHang")]
        public async Task<IActionResult> Details(int id)
        {
            var datLich = await _datLichServices.GetDatLichByIdAsync(id);
            if (datLich == null)
            {
                return NotFound();
            }

            if (!await CoQuyenTruyCapLichAsync(id))
            {
                return Forbid();
            }

            var chiTietDichVus = await _chiTietDatLichServices.GetByDatLichIdAsync(id);
            var hoaDon = await _hoaDonService.GetByDatLichAsync(id);

            ViewBag.ChiTietDichVus = chiTietDichVus;
            ViewBag.HoaDon = hoaDon;

            return View(datLich);
        }

        [HttpGet]
        [Authorize(Roles = "KhachHang")]
        public async Task<IActionResult> Create()
        {

            var model = new CreateDatLichViewModel
            {
                NgayHenLich = DateTime.Today
            };
            await LoadCreateData(model);
            return View(model);
        }
        private async Task LoadCreateData(CreateDatLichViewModel model)
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
            if (model.NgayHenLich == default)
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
                if (datLich.IdDichVu == null || !datLich.IdDichVu.Any())
                {
                    ModelState.AddModelError("IdDichVu", "Vui lòng chọn ít nhất một dịch vụ.");
                    await LoadCreateData(datLich);
                    return View(datLich);
                }
                if (datLich.IdNhanVien <= 0)
                {
                    ModelState.AddModelError("IdNhanVien", "Vui lòng chọn nhân viên phù hợp.");
                    await LoadCreateData(datLich);
                    return View(datLich);
                }
                var danhsachDichVu = await _dichVuServices.GetDichVusByIdAsync(datLich.IdDichVu);
                if (danhsachDichVu == null || !danhsachDichVu.Any())
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
                foreach (var dichvu in danhsachDichVu)
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
        [HttpGet]
        [Authorize(Roles = "Admin,KhachHang")]
        public async Task<IActionResult> EditKhachHang(int id)
        {
            var datLich = await _datLichServices.GetDatLichByIdAsync(id);
            if (datLich == null) return NotFound();

            if (User.IsInRole("KhachHang"))
            {
                var khachHangId = GetCurrentKhachHangId();
                if (khachHangId == null || datLich.IdKhachHang != khachHangId.Value)
                    return Forbid();
            }
            if (datLich.TrangThaiDatLich == TrangThaiDatLich.DaHoanThanh || datLich.TrangThaiDatLich == TrangThaiDatLich.DaHuy)
            {
                TempData["ErrorMessage"] =
                    "Lịch đã hoàn thành hoặc đã hủy nên không thể chỉnh sửa.";

                return RedirectToAction(nameof(Index));
            }
            var model = new KhachHangDatLichViewModel
            {
                IdDatLich = datLich.IdDatLich,
                NgayHenLich = datLich.NgayHenLich,
                GhiChuDatLich = datLich.GhiChuDatLich
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,KhachHang")]
        public async Task<IActionResult> EditKhachHang(int id, KhachHangDatLichViewModel model)
        {
            if (id != model.IdDatLich) return NotFound();

            if (!ModelState.IsValid) return View(model);

            var datLichEntity = await _datLichServices.GetDatLichByIdAsync(model.IdDatLich);
            if (datLichEntity == null) return NotFound();

            if (User.IsInRole("KhachHang"))
            {
                var khachHangId = GetCurrentKhachHangId();
                if (khachHangId == null || datLichEntity.IdKhachHang != khachHangId.Value)
                    return Forbid();
            }
            if (datLichEntity.TrangThaiDatLich == TrangThaiDatLich.DaHoanThanh || datLichEntity.TrangThaiDatLich == TrangThaiDatLich.DaHuy)
            {
                TempData["ErrorMessage"] =
                    "Lịch đã hoàn thành hoặc đã hủy nên không thể chỉnh sửa.";

                return RedirectToAction(nameof(Index));
            }

            datLichEntity.NgayHenLich = model.NgayHenLich;
            datLichEntity.GhiChuDatLich = model.GhiChuDatLich;

            await _datLichServices.UpdateDatLichAsync(datLichEntity);

            TempData["SuccessMessage"] = "Cập nhật đặt lịch thành công.";
            return RedirectToAction(nameof(Index));
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
            if (datLich.TrangThaiDatLich == TrangThaiDatLich.DaHoanThanh || datLich.TrangThaiDatLich == TrangThaiDatLich.DaHuy)
            {
                TempData["ErrorMessage"] =
                    "Lịch đã hoàn thành hoặc đã hủy nên không thể cập nhật.";

                return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> EditNhanVien(int id, NhanVienDatLichViewModel model)
        {
            if (id != model.IdDatLich) return NotFound();

            if (!ModelState.IsValid) return View(model);

            var datLichEntity = await _datLichServices.GetDatLichByIdAsync(model.IdDatLich);
            if (datLichEntity == null) return NotFound();

            if (datLichEntity.TrangThaiDatLich == TrangThaiDatLich.DaHoanThanh || datLichEntity.TrangThaiDatLich == TrangThaiDatLich.DaHuy)
            {
                TempData["ErrorMessage"] =
                    "Lịch đã hoàn thành hoặc đã hủy nên không thể cập nhật.";

                return RedirectToAction(nameof(Index));
            }

            if (User.IsInRole("NhanVien"))
            {
                var nhanVienId = GetCurrentNhanVienId();
                if (nhanVienId == null || datLichEntity.IdNhanVien != nhanVienId.Value)
                    return Forbid();
            }

            if (User.IsInRole("Admin"))
            {
                datLichEntity.IdNhanVien = model.IdNhanVien;
                datLichEntity.NgayHenLich = model.NgayHenLich;
                datLichEntity.GioBatDauDV = model.GioBatDauDV;
                datLichEntity.GioKetThucDV = model.GioKetThucDV;
            }

            datLichEntity.TrangThaiDatLich = model.TrangThaiDatLich;
            datLichEntity.GhiChuDatLich = model.GhiChuDatLich;

            await _datLichServices.UpdateDatLichAsync(datLichEntity);

            TempData["SuccessMessage"] = "Cập nhật đặt lịch thành công.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,NhanVien")]
        public async Task<IActionResult> UpdateTrangThai(int id, TrangThaiDatLich trangThai)
        {
            if (!await CoQuyenTruyCapLichAsync(id))
                return Forbid();

            var datLich = await _datLichServices.GetDatLichByIdAsync(id);

            if (datLich.TrangThaiDatLich == TrangThaiDatLich.DaHoanThanh ||
                datLich.TrangThaiDatLich == TrangThaiDatLich.DaHuy)
            {
                TempData["ErrorMessage"] =
                    "Lịch đã hoàn thành hoặc đã hủy nên không thể cập nhật trạng thái.";

                return RedirectToAction(nameof(Index));
            }

            await _datLichServices.UpdateTrangThaiAsync(id, trangThai);

            TempData["SuccessMessage"] = "Cập nhật trạng thái thành công.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,NhanVien")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!await CoQuyenTruyCapLichAsync(id))
                {
                    return Forbid();
                }

                var datLich = await _datLichServices.GetDatLichByIdAsync(id);

                if (datLich.TrangThaiDatLich == TrangThaiDatLich.DaHoanThanh ||
                    datLich.TrangThaiDatLich == TrangThaiDatLich.DaHuy)
                {
                    TempData["ErrorMessage"] =
                        "Lịch đã hoàn thành hoặc đã hủy nên không thể hủy.";

                    return RedirectToAction(nameof(Index));
                }

                await _datLichServices.HuyDatLichAsync(id);

                TempData["SuccessMessage"] = "Hủy đặt lịch thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        [Authorize(Roles = "KhachHang")]
        public async Task<IActionResult> GetNhanVienPhuHop(
     DateTime ngayHen,
     string gioBatDau,
     [FromQuery] List<int> idDichVus)
        {
            if (string.IsNullOrWhiteSpace(gioBatDau))
            {
                return Json(new List<object>());
            }

            if (!TimeSpan.TryParse(gioBatDau, out var gio))
            {
                return Json(new List<object>());
            }

            if (idDichVus == null || !idDichVus.Any())
            {
                return Json(new List<object>());
            }

            var dichVus = await _dichVuServices.GetDichVusByIdAsync(idDichVus);

            if (dichVus == null || !dichVus.Any())
            {
                return Json(new List<object>());
            }

            var tongThoiLuong = dichVus.Sum(x => x.ThoiLuongDV);

            var gioBatDauDateTime = ngayHen.Date.Add(gio);
            var gioKetThucDateTime = gioBatDauDateTime.AddMinutes(tongThoiLuong);

            var nhanViens = await _datLichServices.GetNhanVienPhuHopAsync(
                idDichVus,
                ngayHen,
                gioBatDauDateTime,
                gioKetThucDateTime
            );

            var result = nhanViens.Select(x => new
            {
                idNhanVien = x.IdNhanVien,
                tenNhanVien = x.TenNhanVien,
                chuyenMonNV = HienThiChuyenMon(x.ChuyenMonNV)
            }).ToList();

            return Json(result);
        }
        private string HienThiChuyenMon(string? chuyenMon)
        {
            return chuyenMon switch
            {
                "ChamSocDa" => "Chăm sóc da",
                "Massage" => "Massage",
                "GoiDau" => "Gội đầu dưỡng sinh",
                "Body" => "Chăm sóc body",
                _ => chuyenMon ?? "Nhân viên spa"
            };
        }
        private int? GetCurrentNhanVienId()
        {
            var claim = User.FindFirst("IdNhanVien")?.Value;
            return int.TryParse(claim, out var id) ? id : null;
        }

        private int? GetCurrentKhachHangId()
        {
            var claim = User.FindFirst("IdKhachHang")?.Value;
            return int.TryParse(claim, out var id) ? id : null;
        }

        private async Task<bool> CoQuyenTruyCapLichAsync(int idDatLich)
        {
            var datLich = await _datLichServices.GetDatLichByIdAsync(idDatLich);
            if (datLich == null) return false;

            if (User.IsInRole("Admin")) return true;

            if (User.IsInRole("NhanVien"))
            {
                var idNhanVien = GetCurrentNhanVienId();
                return idNhanVien != null && datLich.IdNhanVien == idNhanVien.Value;
            }

            if (User.IsInRole("KhachHang"))
            {
                var idKhachHang = GetCurrentKhachHangId();
                return idKhachHang != null && datLich.IdKhachHang == idKhachHang.Value;
            }

            return false;
        }
    }
}
