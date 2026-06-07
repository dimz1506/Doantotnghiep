using Doantotnghiep.Models.Entities;
using Doantotnghiep.Models.Enum;
using Doantotnghiep.Repositories.Interface;
using Doantotnghiep.Services.Interfaces;

namespace Doantotnghiep.Services.Implementations
{
    public class DatLichServices : IDatLichServices
    {
        private readonly IDatLichRepository _datLichRepository;
        public DatLichServices(IDatLichRepository datLichRepository)
        {
            _datLichRepository = datLichRepository;
        }

        public async Task<DatLich> CreateDatLichAsync(DatLich datLich)
        {
            //kiem tra rong
            if (datLich == null)
            {
                throw new ArgumentNullException(nameof(datLich), "Đặt lịch không được để trống.");
            }
            //kiem tra giờ bắt đầu phải nhỏ hơn giờ kết thúc
            if (datLich.GioBatDauDV >= datLich.GioKetThucDV)
            {
                throw new ArgumentException("Giờ bắt đầu dịch vụ phải nhỏ hơn giờ kết thúc dịch vụ.");
            }
            //check ngay hen lich phai lon hon ngay hien tai
            if (datLich.NgayHenLich.Date < DateTime.Now.Date)
            {
                throw new ArgumentException("Ngày hẹn lịch phải lớn hơn hoặc bằng ngày hiện tại.");
            }
            //check trung
            var isTrungLich = await _datLichRepository.KiemTraTrungLichAsync(datLich.IdNhanVien, datLich.NgayHenLich, datLich.GioBatDauDV, datLich.GioKetThucDV, null);
            if (isTrungLich)
            {
                throw new InvalidOperationException("Lịch hẹn bị trùng với lịch đã tồn tại.");
            }
            //set trang thai
            datLich.TrangThaiDatLich = TrangThaiDatLich.ChoXacNhan;
            //set ngay tao lich
            datLich.NgayTaoDatLich = DateTime.Now;
            //set ngay cap nhat lich
            datLich.NgayCapNhatDatLich = DateTime.Now;
            //luu dat lich
            var datLich1 = await _datLichRepository.CreateDatLichAsync(datLich);
            return datLich1;
        }

        public async Task<List<DatLich>> GetAllDatLichAsync()
        {
            var datLichs = await _datLichRepository.GetAllDatLichAsync();
            return datLichs;
        }

        public async Task<DatLich?> GetDatLichByIdAsync(int id)
        {
            //kiem tra id
            if (id <= 0)
            {
                throw new ArgumentException("Id không hợp lệ.");
            }
            var datLich = await _datLichRepository.GetDatLichByIdAsync(id);
            if (datLich == null)
            {
                throw new KeyNotFoundException("Không tìm thấy đặt lịch với Id đã cho.");
            }
            return datLich;
        }

        public async Task<List<DatLich>> GetDatLichByKhachHangIdAsync(int khachHangId)
        {
            //kiem tra id
            if (khachHangId <= 0)
            {
                throw new ArgumentException("Id khách hàng không hợp lệ.");
            }
            var datLichs = await _datLichRepository.GetDatLichByKhachHangIdAsync(khachHangId);
            return datLichs;
        }

        public async Task<List<DatLich>> GetDatLichByNgayHenLichAsync(DateTime ngayHenLich)
        {
            var datLichs = await _datLichRepository.GetDatLichByNgayHenLichAsync(ngayHenLich);
            return datLichs;
        }

        public async Task<List<DatLich>> GetDatLichByNhanVienIdAsync(int nhanVienId)
        {
            //kiem tra id
            if (nhanVienId <= 0)
            {
                throw new ArgumentException("Id nhân viên không hợp lệ.");
            }
            var datLichs = await _datLichRepository.GetDatLichByNhanVienIdAsync(nhanVienId);
            return datLichs;
        }

        public async Task<List<DatLich>> GetDatLichByTrangThaiAsync(TrangThaiDatLich trangThai)
        {
            //kiem tra trang thai
            if (!Enum.IsDefined(typeof(TrangThaiDatLich), trangThai))
            {
                throw new ArgumentException("Trạng thái đặt lịch không hợp lệ.");
            }

            var datLichs = await _datLichRepository.GetDatLichByTrangThaiAsync(trangThai);
            return datLichs;
        }

        public async Task<bool> KiemTraTrungLichAsync(int IdNhanVien, DateTime ngayHenLich, DateTime gioBatDauDV, DateTime gioKetThucDV, int? IdDatLich)
        {
            //kiem tra id nhan vien
            if (IdNhanVien <= 0)
            {
                throw new ArgumentException("Id nhân viên không hợp lệ.");
            }
            //kiem tra ngay hen lich phai lon hon ngay hien tai
            if (ngayHenLich.Date < DateTime.Now.Date)
            {
                throw new ArgumentException("Ngày hẹn lịch phải lớn hơn hoặc bằng ngày hiện tại.");
            }
            //kiem tra gio bat dau phai nho hon gio ket thuc
            if (gioBatDauDV >= gioKetThucDV)
            {
                throw new ArgumentException("Giờ bắt đầu dịch vụ phải nhỏ hơn giờ kết thúc dịch vụ.");
            }

            var isTrungLich = await _datLichRepository.KiemTraTrungLichAsync(IdNhanVien, ngayHenLich, gioBatDauDV, gioKetThucDV, IdDatLich);
            return isTrungLich;
        }

        public async Task<bool> UpdateDatLichAsync(DatLich datLich)
        {
            //kiem tra dat lich null
            if (datLich == null)
            {
                throw new ArgumentNullException(nameof(datLich), "Đặt lịch không được để trống.");
            }
            //kiem tra id dat lich
            if (datLich.IdDatLich <= 0)
            {
                throw new ArgumentException("Id đặt lịch không hợp lệ.");
            }
            //kiem tra gio bat dau phai nho hon gio ket thuc
            if (datLich.GioBatDauDV >= datLich.GioKetThucDV)
            {
                throw new ArgumentException("Giờ bắt đầu dịch vụ phải nhỏ hơn giờ kết thúc dịch vụ.");
            }
            //kiem tra ngay hen lich phai lon hon ngay hien tai
            if (datLich.NgayHenLich.Date < DateTime.Now.Date)
            {
                throw new ArgumentException("Ngày hẹn lịch phải lớn hơn hoặc bằng ngày hiện tại.");
            }
            //kiem tra trung
            var isTrungLich = await _datLichRepository.KiemTraTrungLichAsync(datLich.IdNhanVien, datLich.NgayHenLich, datLich.GioBatDauDV, datLich.GioKetThucDV, datLich.IdDatLich);
            if (isTrungLich)
            {
                throw new InvalidOperationException("Lịch hẹn bị trùng với lịch đã tồn tại.");
            }
            //set ngay cap nhat lich
            datLich.NgayCapNhatDatLich = DateTime.Now;
            //cap nhat dat lich
            var result = await _datLichRepository.UpdateDatLichAsync(datLich);
            if (result == false)
            {
                throw new InvalidOperationException("Lịch đã hoàn thành nên không thể cập nhật hoặc hủy lịch.");
            }
            return true;
        }

        public async Task<bool> UpdateTrangThaiAsync(int iddatlich, TrangThaiDatLich trangThailich)
        {
            //kiem tra id dat lich
            if (iddatlich <= 0)
            {
                throw new ArgumentException("Id đặt lịch không hợp lệ.");
            }

            var result = await _datLichRepository.UpdateTrangThaiAsync(iddatlich, trangThailich);
            if (result == false)
            {
                throw new InvalidOperationException("Lịch đã hoàn thành nên không thể cập nhật hoặc hủy lịch.");
            }
            return true;
        }
        public async Task<List<NhanVien>> GetNhanVienSelectListAsync()
        {

            var nhanViens = await _datLichRepository.GetNhanVienSelectListAsync();
            return nhanViens;
        }
        public async Task<List<DichVu>> GetDichVuSelectListAsync()
        {
            var dichVus = await _datLichRepository.GetDichVuSelectListAsync();
            return dichVus;
        }
        public async Task<List<NhanVien>> GetNhanVienPhuHopAsync(
     List<int> idDichVus,
     DateTime ngayHen,
     DateTime gioBatDau,
     DateTime gioKetThuc)
        {
            var nhanViens = await _datLichRepository.GetNhanVienSelectListAsync();

            var ketQua = new List<NhanVien>();

            foreach (var nv in nhanViens)
            {
                if (nv.NhanVienDichVus == null)
                {
                    continue;
                }

                var dichVuNhanVienPhuTrach = nv.NhanVienDichVus
                    .Select(x => x.IdDichVu)
                    .ToList();

                var coTheLamTatCaDichVu = idDichVus
                    .All(idDichVu => dichVuNhanVienPhuTrach.Contains(idDichVu));

                if (!coTheLamTatCaDichVu)
                {
                    continue;
                }

                if (gioBatDau.TimeOfDay < nv.GioBatDauLamViec ||
                    gioKetThuc.TimeOfDay > nv.GioKetThucLamViec)
                {
                    continue;
                }

                var trungLich = await _datLichRepository.KiemTraTrungLichAsync(
                    nv.IdNhanVien,
                    ngayHen,
                    gioBatDau,
                    gioKetThuc,
                    null
                );

                if (!trungLich)
                {
                    ketQua.Add(nv);
                }
            }

            return ketQua;
        }

        public async Task<bool> HuyDatLichAsync(int iddatlich)
        {
            if (iddatlich <= 0)
            {
                throw new ArgumentException("Id đặt lịch không hợp lệ.");
            }

            var datLich = await _datLichRepository.GetDatLichByIdAsync(iddatlich);

            if (datLich == null)
            {
                throw new KeyNotFoundException("Không tìm thấy đặt lịch.");
            }

            if (datLich.TrangThaiDatLich == TrangThaiDatLich.DaHoanThanh)
            {
                throw new InvalidOperationException("Lịch đã hoàn thành nên không thể hủy.");
            }

            if (datLich.TrangThaiDatLich == TrangThaiDatLich.DaHuy)
            {
                throw new InvalidOperationException("Lịch này đã được hủy trước đó.");
            }

            var result = await _datLichRepository.UpdateTrangThaiAsync(
                iddatlich,
                TrangThaiDatLich.DaHuy
            );

            if (!result)
            {
                throw new InvalidOperationException("Hủy lịch thất bại.");
            }

            return true;
        }
    }
}
