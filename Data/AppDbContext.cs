using Microsoft.EntityFrameworkCore;
using Doantotnghiep.Models.Entities;
namespace Doantotnghiep.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<VaiTro> VaiTros { get; set; }
        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<LoaiDichVu> LoaiDichVus { get; set; }
        public DbSet<DichVu> DichVus { get; set; }
        public DbSet<NhanVienDichVu> NhanVienDichVus { get; set; }
        public DbSet<DatLich> DatLichs { get; set; }
        public DbSet<LichLamViecNhanVien> LichLamViecNhanViens { get; set; }
        public DbSet<HoiThoaiAI> HoiThoaiAIs { get; set; }
        public DbSet<LoaiTriThuc> LoaiTriThucs { get; set; }
        public DbSet<TriThuc> TriThucs { get; set; }
        public DbSet<KhuyenMai> KhuyenMais { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public DbSet<DichVuKhuyenMai> DichVuKhuyenMais { get; set; }
        public DbSet<ChiTietDatLich> ChiTietDatLiches { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DatLich>()
                .HasOne(dl => dl.KhachHang)
                .WithMany(kh => kh.DatLiches)
                .HasForeignKey(dl => dl.IdKhachHang)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<DatLich>()
                .HasOne(dl => dl.NhanVien)
                .WithMany(nv => nv.DatLiches)
                .HasForeignKey(dl => dl.IdNhanVien)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<NhanVienDichVu>()
                .HasOne(nvdv => nvdv.NhanVien)
                .WithMany(nv => nv.NhanVienDichVus)
                .HasForeignKey(nvdv => nvdv.IdNhanVien)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<NhanVienDichVu>()
                .HasOne(nvdv => nvdv.DichVu)
                .WithMany(dv => dv.NhanVienDichVus)
                .HasForeignKey(nvdv => nvdv.IdDichVu)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ChiTietHoaDon>()
                .HasOne(cthd => cthd.DichVu)
                .WithMany(dv => dv.ChiTietHoaDons)
                .HasForeignKey(cthd => cthd.IdDichVu)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ChiTietHoaDon>()
               .HasOne(cthd => cthd.HoaDon)
               .WithMany(hd => hd.ChiTietHoaDons)
               .HasForeignKey(cthd => cthd.IdHoaDon)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<DichVuKhuyenMai>()
               .HasOne(dvkm => dvkm.DichVu)
               .WithMany(dv => dv.DichVuKhuyenMais)
               .HasForeignKey(dvkm => dvkm.IdDichVu)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<DichVuKhuyenMai>()
               .HasOne(dvkm => dvkm.KhuyenMai)
               .WithMany(km => km.DichVuKhuyenMais)
               .HasForeignKey(dvkm => dvkm.IdKhuyenMai)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<HoaDon>()
               .HasOne(hd => hd.DatLich)
               .WithMany(dl => dl.HoaDons)
               .HasForeignKey(hd => hd.IdDatLich)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ChiTietDatLich>()
               .HasOne(ctdl => ctdl.DatLich)
               .WithMany(dl => dl.ChiTietDatLichs)
               .HasForeignKey(ctdl => ctdl.IdDatLich)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ChiTietDatLich>()
               .HasOne(ctdl => ctdl.DichVu)
               .WithMany(dv => dv.ChiTietDatLichs)
               .HasForeignKey(ctdl => ctdl.IdDichVu)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<KhachHang>()
               .HasOne(kh => kh.TaiKhoan)
               .WithOne(tk => tk.KhachHang)
               .HasForeignKey<KhachHang>(tk => tk.IdTaiKhoan)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<NhanVien>()
               .HasOne(nv => nv.TaiKhoan)
               .WithOne(tk => tk.NhanVien)
               .HasForeignKey<NhanVien>(tk => tk.IdTaiKhoan)
               .OnDelete(DeleteBehavior.Restrict);

        }
    }

}