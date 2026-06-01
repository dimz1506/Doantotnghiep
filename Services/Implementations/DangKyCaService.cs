using Doantotnghiep.Models.Entities;
using Doantotnghiep.Models.Enum;
using Doantotnghiep.Models.ViewModel;
using Doantotnghiep.Repositories.Interface;
using Doantotnghiep.Services.Interfaces;

namespace Doantotnghiep.Services.Implementations
{
    public class DangKyCaService : IDangKyCaLamViecService
    {
        private readonly IDangKyCaLamRepository _dangkyRepository;
        private readonly ILichlamViecRepository _lichlamViecRepository;
        public DangKyCaService(IDangKyCaLamRepository dangkyRepository, ILichlamViecRepository lichlamViecRepository)
        {
            _dangkyRepository = dangkyRepository;
            _lichlamViecRepository = lichlamViecRepository;
        }

        public async Task DangKyCaAsync(DangKyCaViewModel viewModel)
        {
            var entity = new DangKyCaLamViec
            {
                IdNhanVien = viewModel.IdNhanvien,
                IdCaLamViec = viewModel.IdCaLamViec,
                TrangThai = TrangThaiDangKyCaLam.ChoDuyet
            };

            await _dangkyRepository.AddAsync(entity);
        }

        public async Task DuyetCaAsync(int id)
        {
            var dangKy = await _dangkyRepository.GetByIdAsync(id);
            if(dangKy == null)
            {
                return;
            }
            if (dangKy.TrangThai == TrangThaiDangKyCaLam.DaDuyet)
            {
                return;
            }

            if (dangKy.CaLamViec == null)
            {
                throw new Exception("Không tìm thấy thông tin ca làm việc.");
            }
            dangKy.TrangThai = TrangThaiDangKyCaLam.DaDuyet;
            await _dangkyRepository.UpdateAsync(dangKy);

            var lich = new LichLamViecNhanVien
            {
                IdNhanVien = dangKy.IdNhanVien,
                IdCaLamViec = dangKy.IdCaLamViec,
                NgayLamViecNV = dangKy.CaLamViec.NgayLam,
                GioBatDauCaLamViecNV = dangKy.CaLamViec.GioBatDau,
                GioKetThucCaLamViecNV = dangKy.CaLamViec.GioKetThuc,
                TrangThaiLichLamViecNV = TrangThaiLichLamViec.DangLam
            };
            await _lichlamViecRepository.AddAsync(lich);
        }

        public async Task<List<LichLamViecViewModel>> GetAllDangKyCaAsync()
        {
            var data = await _dangkyRepository.GetAllAsync();
            return data.Select(x => new LichLamViecViewModel
            {
                IdLichLamViec = x.IdDangKyca,
                TenNhanVien = x.NhanVien!.TenNhanVien,
                NgayLamViec = x.CaLamViec != null ? x.CaLamViec.NgayLam : DateTime.MinValue,
                GioBatDauLV = x.CaLamViec != null ? x.CaLamViec.GioBatDau : DateTime.MinValue,
                GioKetThucLV = x.CaLamViec != null ? x.CaLamViec.GioKetThuc : DateTime.MinValue,
                TrangThaiDangKyCaLam = x.TrangThai
            }).ToList(); 
        }  

        public async Task<List<LichLamViecViewModel>> GetDangKyByNhanVienAsync(int idNhanVien)
        {
            var data = await _dangkyRepository.GetByNhanVienAsync(idNhanVien);
            return data.Select(x => new LichLamViecViewModel
            {
                IdLichLamViec = x.IdDangKyca,
                IdNhanVien = x.IdNhanVien,
                TenNhanVien = x.NhanVien != null ? x.NhanVien.TenNhanVien : "",
                NgayLamViec = x.CaLamViec != null ? x.CaLamViec.NgayLam : DateTime.MinValue,
                GioBatDauLV = x.CaLamViec != null ? x.CaLamViec.GioBatDau : DateTime.MinValue,
                GioKetThucLV = x.CaLamViec != null ? x.CaLamViec.GioKetThuc : DateTime.MinValue,
                TrangThaiDangKyCaLam = x.TrangThai
            }).ToList();
        }

        public async Task<List<LichLamViecViewModel>> GetLichLamByNhanVienAsync(int idNhanVien)
        {
            var data = await _lichlamViecRepository.GetByNhanVienAsync(idNhanVien);
            return data.Select(x => new LichLamViecViewModel
            {
                IdLichLamViec = x.IdLichLamViecNhanVien,
                IdNhanVien = x.IdNhanVien,
                TenNhanVien = x.NhanVien != null ? x.NhanVien.TenNhanVien : "",
                NgayLamViec = x.NgayLamViecNV,
                GioBatDauLV = x.GioBatDauCaLamViecNV,
                GioKetThucLV = x.GioKetThucCaLamViecNV
            }).ToList();

        }

        public async Task TuChoiAsync(int id)
        {
            var dangKy = await _dangkyRepository.GetByIdAsync(id);
            if(dangKy == null)
            {
                return;
            }
            dangKy.TrangThai = TrangThaiDangKyCaLam.TuChoi;
            await _dangkyRepository.UpdateAsync(dangKy);

        }
    }
}
