using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doantotnghiep.Migrations
{
    /// <inheritdoc />
    public partial class upateLoaitrithuc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoiThoaiAIs_KhachHangs_IdKhachHang",
                table: "HoiThoaiAIs");

            migrationBuilder.DropForeignKey(
                name: "FK_HoiThoaiAIs_NhanViens_NhanVienIdNhanVien",
                table: "HoiThoaiAIs");

            migrationBuilder.DropIndex(
                name: "IX_HoiThoaiAIs_NhanVienIdNhanVien",
                table: "HoiThoaiAIs");

            migrationBuilder.DropColumn(
                name: "NhanVienIdNhanVien",
                table: "HoiThoaiAIs");

            migrationBuilder.DropColumn(
                name: "NoiDungHoiThoai",
                table: "HoiThoaiAIs");

            migrationBuilder.RenameColumn(
                name: "TrangThaiChuyenNV",
                table: "HoiThoaiAIs",
                newName: "CanNhanVienTuVan");

            migrationBuilder.RenameColumn(
                name: "ThoiGianHoiThoai",
                table: "HoiThoaiAIs",
                newName: "ThoiGianBatDau");

            migrationBuilder.RenameColumn(
                name: "NoiDungTraLoi",
                table: "HoiThoaiAIs",
                newName: "TrangThaiHoiThoai");

            migrationBuilder.AlterColumn<string>(
                name: "MoTaLoaiTriThuc",
                table: "LoaiTriThucs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "LoaiTriThucs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayCapNhatLoaiTriThuc",
                table: "LoaiTriThucs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayTaoLoaiTriThuc",
                table: "LoaiTriThucs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayXoaLoaiTriThuc",
                table: "LoaiTriThucs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdNhanVien",
                table: "HoiThoaiAIs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IdKhachHang",
                table: "HoiThoaiAIs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "GhiChuNoiBo",
                table: "HoiThoaiAIs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiGianKetThuc",
                table: "HoiThoaiAIs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoiThoaiAIs_IdNhanVien",
                table: "HoiThoaiAIs",
                column: "IdNhanVien");

            migrationBuilder.AddForeignKey(
                name: "FK_HoiThoaiAIs_KhachHangs_IdKhachHang",
                table: "HoiThoaiAIs",
                column: "IdKhachHang",
                principalTable: "KhachHangs",
                principalColumn: "IdKhachHang");

            migrationBuilder.AddForeignKey(
                name: "FK_HoiThoaiAIs_NhanViens_IdNhanVien",
                table: "HoiThoaiAIs",
                column: "IdNhanVien",
                principalTable: "NhanViens",
                principalColumn: "IdNhanVien");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoiThoaiAIs_KhachHangs_IdKhachHang",
                table: "HoiThoaiAIs");

            migrationBuilder.DropForeignKey(
                name: "FK_HoiThoaiAIs_NhanViens_IdNhanVien",
                table: "HoiThoaiAIs");

            migrationBuilder.DropIndex(
                name: "IX_HoiThoaiAIs_IdNhanVien",
                table: "HoiThoaiAIs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "LoaiTriThucs");

            migrationBuilder.DropColumn(
                name: "NgayCapNhatLoaiTriThuc",
                table: "LoaiTriThucs");

            migrationBuilder.DropColumn(
                name: "NgayTaoLoaiTriThuc",
                table: "LoaiTriThucs");

            migrationBuilder.DropColumn(
                name: "NgayXoaLoaiTriThuc",
                table: "LoaiTriThucs");

            migrationBuilder.DropColumn(
                name: "GhiChuNoiBo",
                table: "HoiThoaiAIs");

            migrationBuilder.DropColumn(
                name: "ThoiGianKetThuc",
                table: "HoiThoaiAIs");

            migrationBuilder.RenameColumn(
                name: "TrangThaiHoiThoai",
                table: "HoiThoaiAIs",
                newName: "NoiDungTraLoi");

            migrationBuilder.RenameColumn(
                name: "ThoiGianBatDau",
                table: "HoiThoaiAIs",
                newName: "ThoiGianHoiThoai");

            migrationBuilder.RenameColumn(
                name: "CanNhanVienTuVan",
                table: "HoiThoaiAIs",
                newName: "TrangThaiChuyenNV");

            migrationBuilder.AlterColumn<string>(
                name: "MoTaLoaiTriThuc",
                table: "LoaiTriThucs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdNhanVien",
                table: "HoiThoaiAIs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdKhachHang",
                table: "HoiThoaiAIs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NhanVienIdNhanVien",
                table: "HoiThoaiAIs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoiDungHoiThoai",
                table: "HoiThoaiAIs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_HoiThoaiAIs_NhanVienIdNhanVien",
                table: "HoiThoaiAIs",
                column: "NhanVienIdNhanVien");

            migrationBuilder.AddForeignKey(
                name: "FK_HoiThoaiAIs_KhachHangs_IdKhachHang",
                table: "HoiThoaiAIs",
                column: "IdKhachHang",
                principalTable: "KhachHangs",
                principalColumn: "IdKhachHang",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HoiThoaiAIs_NhanViens_NhanVienIdNhanVien",
                table: "HoiThoaiAIs",
                column: "NhanVienIdNhanVien",
                principalTable: "NhanViens",
                principalColumn: "IdNhanVien");
        }
    }
}
