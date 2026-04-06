using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doantotnghiep.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KhuyenMais",
                columns: table => new
                {
                    IdKhuyenMai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenKhuyenMai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTaKhuyenMai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhanTramGiamGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrangThaiKhuyenMai = table.Column<bool>(type: "bit", nullable: false),
                    NgayTaoKhuyenMai = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhatKhuyenMai = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    NgayXoaKhuyenMai = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhuyenMais", x => x.IdKhuyenMai);
                });

            migrationBuilder.CreateTable(
                name: "LoaiDichVus",
                columns: table => new
                {
                    IdLoaiDichVu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoaiDichVu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTaLoaiDichVu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThaiLoaiDV = table.Column<bool>(type: "bit", nullable: false),
                    NgayTaoLoaiDichVu = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiDichVus", x => x.IdLoaiDichVu);
                });

            migrationBuilder.CreateTable(
                name: "LoaiTriThucs",
                columns: table => new
                {
                    IdLoaiTriThuc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoaiTriThuc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTaLoaiTriThuc = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiTriThucs", x => x.IdLoaiTriThuc);
                });

            migrationBuilder.CreateTable(
                name: "VaiTros",
                columns: table => new
                {
                    IdVaiTro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenVaiTro = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaiTros", x => x.IdVaiTro);
                });

            migrationBuilder.CreateTable(
                name: "DichVus",
                columns: table => new
                {
                    IdDichVu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdLoaiDichVu = table.Column<int>(type: "int", nullable: false),
                    TenDichVu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTaDichVu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GiaDichVu = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrangThaiDV = table.Column<bool>(type: "bit", nullable: false),
                    ThoiLuongDV = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    NgayTaoDichVu = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayXoaDV = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HinhAnhDichVu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DichVus", x => x.IdDichVu);
                    table.ForeignKey(
                        name: "FK_DichVus_LoaiDichVus_IdLoaiDichVu",
                        column: x => x.IdLoaiDichVu,
                        principalTable: "LoaiDichVus",
                        principalColumn: "IdLoaiDichVu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TriThucs",
                columns: table => new
                {
                    IdTriThuc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTriThuc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoiDungTriThuc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayNhapTriThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhatTriThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdLoaiTriThuc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TriThucs", x => x.IdTriThuc);
                    table.ForeignKey(
                        name: "FK_TriThucs_LoaiTriThucs_IdLoaiTriThuc",
                        column: x => x.IdLoaiTriThuc,
                        principalTable: "LoaiTriThucs",
                        principalColumn: "IdLoaiTriThuc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoans",
                columns: table => new
                {
                    IdTaiKhoan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTaiKhoan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Passwordhash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdVaiTro = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThaiTK = table.Column<bool>(type: "bit", nullable: false),
                    NgayTaoTaiKhoan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayXoaTaiKhoan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoans", x => x.IdTaiKhoan);
                    table.ForeignKey(
                        name: "FK_TaiKhoans_VaiTros_IdVaiTro",
                        column: x => x.IdVaiTro,
                        principalTable: "VaiTros",
                        principalColumn: "IdVaiTro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DichVuKhuyenMais",
                columns: table => new
                {
                    IdDichVuKhuyenMai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdDichVu = table.Column<int>(type: "int", nullable: false),
                    IdKhuyenMai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DichVuKhuyenMais", x => x.IdDichVuKhuyenMai);
                    table.ForeignKey(
                        name: "FK_DichVuKhuyenMais_DichVus_IdDichVu",
                        column: x => x.IdDichVu,
                        principalTable: "DichVus",
                        principalColumn: "IdDichVu");
                    table.ForeignKey(
                        name: "FK_DichVuKhuyenMais_KhuyenMais_IdKhuyenMai",
                        column: x => x.IdKhuyenMai,
                        principalTable: "KhuyenMais",
                        principalColumn: "IdKhuyenMai");
                });

            migrationBuilder.CreateTable(
                name: "KhachHangs",
                columns: table => new
                {
                    IdKhachHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTaiKhoan = table.Column<int>(type: "int", nullable: false),
                    TenKhachHang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChiKhachHang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GhiChuKH = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayTaoKH = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    NgayXoaKH = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHangs", x => x.IdKhachHang);
                    table.ForeignKey(
                        name: "FK_KhachHangs_TaiKhoans_IdTaiKhoan",
                        column: x => x.IdTaiKhoan,
                        principalTable: "TaiKhoans",
                        principalColumn: "IdTaiKhoan");
                });

            migrationBuilder.CreateTable(
                name: "NhanViens",
                columns: table => new
                {
                    IdNhanVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTaiKhoan = table.Column<int>(type: "int", nullable: false),
                    TenNhanVien = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChiNV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChuyenMonNV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayTaoNV = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThaiNV = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    NgayXoaNV = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanViens", x => x.IdNhanVien);
                    table.ForeignKey(
                        name: "FK_NhanViens_TaiKhoans_IdTaiKhoan",
                        column: x => x.IdTaiKhoan,
                        principalTable: "TaiKhoans",
                        principalColumn: "IdTaiKhoan",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HoiThoaiAIs",
                columns: table => new
                {
                    IdHoiThoaiAI = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdKhachHang = table.Column<int>(type: "int", nullable: false),
                    NoiDungHoiThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoiDungTraLoi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThoiGianHoiThoai = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThaiChuyenNV = table.Column<bool>(type: "bit", nullable: false),
                    IdNhanVien = table.Column<int>(type: "int", nullable: false),
                    NhanVienIdNhanVien = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoiThoaiAIs", x => x.IdHoiThoaiAI);
                    table.ForeignKey(
                        name: "FK_HoiThoaiAIs_KhachHangs_IdKhachHang",
                        column: x => x.IdKhachHang,
                        principalTable: "KhachHangs",
                        principalColumn: "IdKhachHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoiThoaiAIs_NhanViens_NhanVienIdNhanVien",
                        column: x => x.NhanVienIdNhanVien,
                        principalTable: "NhanViens",
                        principalColumn: "IdNhanVien");
                });

            migrationBuilder.CreateTable(
                name: "LichLamViecNhanViens",
                columns: table => new
                {
                    IdLichLamViecNhanVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdNhanVien = table.Column<int>(type: "int", nullable: false),
                    NgayLamViecNV = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioBatDauCaLamViecNV = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioKetThucCaLamViecNV = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThaiLichLamViecNV = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichLamViecNhanViens", x => x.IdLichLamViecNhanVien);
                    table.ForeignKey(
                        name: "FK_LichLamViecNhanViens_NhanViens_IdNhanVien",
                        column: x => x.IdNhanVien,
                        principalTable: "NhanViens",
                        principalColumn: "IdNhanVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NhanVienDichVus",
                columns: table => new
                {
                    IdNhanVienDichVu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdNhanVien = table.Column<int>(type: "int", nullable: false),
                    IdDichVu = table.Column<int>(type: "int", nullable: false),
                    TrangThaiNVDV = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanVienDichVus", x => x.IdNhanVienDichVu);
                    table.ForeignKey(
                        name: "FK_NhanVienDichVus_DichVus_IdDichVu",
                        column: x => x.IdDichVu,
                        principalTable: "DichVus",
                        principalColumn: "IdDichVu");
                    table.ForeignKey(
                        name: "FK_NhanVienDichVus_NhanViens_IdNhanVien",
                        column: x => x.IdNhanVien,
                        principalTable: "NhanViens",
                        principalColumn: "IdNhanVien");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDatLiches",
                columns: table => new
                {
                    IdChiTietDatLich = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdDatLich = table.Column<int>(type: "int", nullable: false),
                    IdDichVu = table.Column<int>(type: "int", nullable: false),
                    GiaDichVu = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDatLiches", x => x.IdChiTietDatLich);
                    table.ForeignKey(
                        name: "FK_ChiTietDatLiches_DichVus_IdDichVu",
                        column: x => x.IdDichVu,
                        principalTable: "DichVus",
                        principalColumn: "IdDichVu");
                });

            migrationBuilder.CreateTable(
                name: "DatLichs",
                columns: table => new
                {
                    IdDatLich = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdKhachHang = table.Column<int>(type: "int", nullable: false),
                    IdChiTietDatLich = table.Column<int>(type: "int", nullable: false),
                    IdNhanVien = table.Column<int>(type: "int", nullable: false),
                    NgayHenLich = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioBatDauDV = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioKetThucDV = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThaiDatLich = table.Column<int>(type: "int", nullable: false),
                    GhiChuDatLich = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayTaoDatLich = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhatDatLich = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DichVuIdDichVu = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatLichs", x => x.IdDatLich);
                    table.ForeignKey(
                        name: "FK_DatLichs_ChiTietDatLiches_IdChiTietDatLich",
                        column: x => x.IdChiTietDatLich,
                        principalTable: "ChiTietDatLiches",
                        principalColumn: "IdChiTietDatLich",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DatLichs_DichVus_DichVuIdDichVu",
                        column: x => x.DichVuIdDichVu,
                        principalTable: "DichVus",
                        principalColumn: "IdDichVu");
                    table.ForeignKey(
                        name: "FK_DatLichs_KhachHangs_IdKhachHang",
                        column: x => x.IdKhachHang,
                        principalTable: "KhachHangs",
                        principalColumn: "IdKhachHang");
                    table.ForeignKey(
                        name: "FK_DatLichs_NhanViens_IdNhanVien",
                        column: x => x.IdNhanVien,
                        principalTable: "NhanViens",
                        principalColumn: "IdNhanVien");
                });

            migrationBuilder.CreateTable(
                name: "HoaDons",
                columns: table => new
                {
                    IdHoaDon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdDatLich = table.Column<int>(type: "int", nullable: false),
                    TongTienGoc = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TongTienGiamGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TongTienSauGiamGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrangThaiHoaDon = table.Column<int>(type: "int", nullable: false),
                    PhuongThucTT = table.Column<int>(type: "int", nullable: false),
                    NgayThanhToan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayTaoHoaDon = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDons", x => x.IdHoaDon);
                    table.ForeignKey(
                        name: "FK_HoaDons_DatLichs_IdDatLich",
                        column: x => x.IdDatLich,
                        principalTable: "DatLichs",
                        principalColumn: "IdDatLich");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietHoaDons",
                columns: table => new
                {
                    IdChiTietHoaDon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdHoaDon = table.Column<int>(type: "int", nullable: false),
                    IdDichVu = table.Column<int>(type: "int", nullable: false),
                    SoLuongDV = table.Column<int>(type: "int", nullable: false),
                    DonGiaDV = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdKhuyenMai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietHoaDons", x => x.IdChiTietHoaDon);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDons_DichVus_IdDichVu",
                        column: x => x.IdDichVu,
                        principalTable: "DichVus",
                        principalColumn: "IdDichVu");
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDons_HoaDons_IdHoaDon",
                        column: x => x.IdHoaDon,
                        principalTable: "HoaDons",
                        principalColumn: "IdHoaDon");
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDons_KhuyenMais_IdKhuyenMai",
                        column: x => x.IdKhuyenMai,
                        principalTable: "KhuyenMais",
                        principalColumn: "IdKhuyenMai");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDatLiches_IdDatLich",
                table: "ChiTietDatLiches",
                column: "IdDatLich");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDatLiches_IdDichVu",
                table: "ChiTietDatLiches",
                column: "IdDichVu");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDons_IdDichVu",
                table: "ChiTietHoaDons",
                column: "IdDichVu");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDons_IdHoaDon",
                table: "ChiTietHoaDons",
                column: "IdHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDons_IdKhuyenMai",
                table: "ChiTietHoaDons",
                column: "IdKhuyenMai");

            migrationBuilder.CreateIndex(
                name: "IX_DatLichs_DichVuIdDichVu",
                table: "DatLichs",
                column: "DichVuIdDichVu");

            migrationBuilder.CreateIndex(
                name: "IX_DatLichs_IdChiTietDatLich",
                table: "DatLichs",
                column: "IdChiTietDatLich");

            migrationBuilder.CreateIndex(
                name: "IX_DatLichs_IdKhachHang",
                table: "DatLichs",
                column: "IdKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_DatLichs_IdNhanVien",
                table: "DatLichs",
                column: "IdNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_DichVuKhuyenMais_IdDichVu",
                table: "DichVuKhuyenMais",
                column: "IdDichVu");

            migrationBuilder.CreateIndex(
                name: "IX_DichVuKhuyenMais_IdKhuyenMai",
                table: "DichVuKhuyenMais",
                column: "IdKhuyenMai");

            migrationBuilder.CreateIndex(
                name: "IX_DichVus_IdLoaiDichVu",
                table: "DichVus",
                column: "IdLoaiDichVu");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_IdDatLich",
                table: "HoaDons",
                column: "IdDatLich");

            migrationBuilder.CreateIndex(
                name: "IX_HoiThoaiAIs_IdKhachHang",
                table: "HoiThoaiAIs",
                column: "IdKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_HoiThoaiAIs_NhanVienIdNhanVien",
                table: "HoiThoaiAIs",
                column: "NhanVienIdNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_KhachHangs_IdTaiKhoan",
                table: "KhachHangs",
                column: "IdTaiKhoan",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LichLamViecNhanViens_IdNhanVien",
                table: "LichLamViecNhanViens",
                column: "IdNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_NhanVienDichVus_IdDichVu",
                table: "NhanVienDichVus",
                column: "IdDichVu");

            migrationBuilder.CreateIndex(
                name: "IX_NhanVienDichVus_IdNhanVien",
                table: "NhanVienDichVus",
                column: "IdNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_NhanViens_IdTaiKhoan",
                table: "NhanViens",
                column: "IdTaiKhoan",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoans_IdVaiTro",
                table: "TaiKhoans",
                column: "IdVaiTro");

            migrationBuilder.CreateIndex(
                name: "IX_TriThucs_IdLoaiTriThuc",
                table: "TriThucs",
                column: "IdLoaiTriThuc");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDatLiches_DatLichs_IdDatLich",
                table: "ChiTietDatLiches",
                column: "IdDatLich",
                principalTable: "DatLichs",
                principalColumn: "IdDatLich");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDatLiches_DatLichs_IdDatLich",
                table: "ChiTietDatLiches");

            migrationBuilder.DropTable(
                name: "ChiTietHoaDons");

            migrationBuilder.DropTable(
                name: "DichVuKhuyenMais");

            migrationBuilder.DropTable(
                name: "HoiThoaiAIs");

            migrationBuilder.DropTable(
                name: "LichLamViecNhanViens");

            migrationBuilder.DropTable(
                name: "NhanVienDichVus");

            migrationBuilder.DropTable(
                name: "TriThucs");

            migrationBuilder.DropTable(
                name: "HoaDons");

            migrationBuilder.DropTable(
                name: "KhuyenMais");

            migrationBuilder.DropTable(
                name: "LoaiTriThucs");

            migrationBuilder.DropTable(
                name: "DatLichs");

            migrationBuilder.DropTable(
                name: "ChiTietDatLiches");

            migrationBuilder.DropTable(
                name: "KhachHangs");

            migrationBuilder.DropTable(
                name: "NhanViens");

            migrationBuilder.DropTable(
                name: "DichVus");

            migrationBuilder.DropTable(
                name: "TaiKhoans");

            migrationBuilder.DropTable(
                name: "LoaiDichVus");

            migrationBuilder.DropTable(
                name: "VaiTros");
        }
    }
}
