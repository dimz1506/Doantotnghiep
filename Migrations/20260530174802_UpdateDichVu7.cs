using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doantotnghiep.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDichVu7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CongCuSuDung",
                table: "DichVus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LieuTrinhDichVu",
                table: "DichVus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LuuYKhachHang",
                table: "DichVus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NguyenLieuSuDung",
                table: "DichVus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuyTrinhThucHien",
                table: "DichVus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GhiChuDatLich",
                table: "DatLichs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CongCuSuDung",
                table: "DichVus");

            migrationBuilder.DropColumn(
                name: "LieuTrinhDichVu",
                table: "DichVus");

            migrationBuilder.DropColumn(
                name: "LuuYKhachHang",
                table: "DichVus");

            migrationBuilder.DropColumn(
                name: "NguyenLieuSuDung",
                table: "DichVus");

            migrationBuilder.DropColumn(
                name: "QuyTrinhThucHien",
                table: "DichVus");

            migrationBuilder.AlterColumn<string>(
                name: "GhiChuDatLich",
                table: "DatLichs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
