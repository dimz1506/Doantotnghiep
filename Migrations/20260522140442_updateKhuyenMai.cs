using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doantotnghiep.Migrations
{
    /// <inheritdoc />
    public partial class updateKhuyenMai : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhanTramGiamGia",
                table: "KhuyenMais",
                newName: "GiaTriGiam");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayXoaKhuyenMai",
                table: "KhuyenMais",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayCapNhatKhuyenMai",
                table: "KhuyenMais",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "LoaiKhuyenMai",
                table: "KhuyenMais",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoaiKhuyenMai",
                table: "KhuyenMais");

            migrationBuilder.RenameColumn(
                name: "GiaTriGiam",
                table: "KhuyenMais",
                newName: "PhanTramGiamGia");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayXoaKhuyenMai",
                table: "KhuyenMais",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayCapNhatKhuyenMai",
                table: "KhuyenMais",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
