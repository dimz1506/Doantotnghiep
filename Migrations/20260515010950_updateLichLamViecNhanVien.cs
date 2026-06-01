using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doantotnghiep.Migrations
{
    /// <inheritdoc />
    public partial class updateLichLamViecNhanVien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GioKetThuc",
                table: "DangKyCaLamViecs");

            migrationBuilder.DropColumn(
                name: "GiobatDau",
                table: "DangKyCaLamViecs");

            migrationBuilder.DropColumn(
                name: "NgayLam",
                table: "DangKyCaLamViecs");

            migrationBuilder.AddColumn<int>(
                name: "IdCaLamViec",
                table: "LichLamViecNhanViens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdCaLamViec",
                table: "DangKyCaLamViecs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CaLamViecs",
                columns: table => new
                {
                    IdCaLamViec = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenCa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayLam = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoLuongNhanVienToiDa = table.Column<int>(type: "int", nullable: false),
                    MoDangKy = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaLamViecs", x => x.IdCaLamViec);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LichLamViecNhanViens_IdCaLamViec",
                table: "LichLamViecNhanViens",
                column: "IdCaLamViec");

            migrationBuilder.CreateIndex(
                name: "IX_DangKyCaLamViecs_IdCaLamViec",
                table: "DangKyCaLamViecs",
                column: "IdCaLamViec");

            migrationBuilder.AddForeignKey(
                name: "FK_DangKyCaLamViecs_CaLamViecs_IdCaLamViec",
                table: "DangKyCaLamViecs",
                column: "IdCaLamViec",
                principalTable: "CaLamViecs",
                principalColumn: "IdCaLamViec",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LichLamViecNhanViens_CaLamViecs_IdCaLamViec",
                table: "LichLamViecNhanViens",
                column: "IdCaLamViec",
                principalTable: "CaLamViecs",
                principalColumn: "IdCaLamViec",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DangKyCaLamViecs_CaLamViecs_IdCaLamViec",
                table: "DangKyCaLamViecs");

            migrationBuilder.DropForeignKey(
                name: "FK_LichLamViecNhanViens_CaLamViecs_IdCaLamViec",
                table: "LichLamViecNhanViens");

            migrationBuilder.DropTable(
                name: "CaLamViecs");

            migrationBuilder.DropIndex(
                name: "IX_LichLamViecNhanViens_IdCaLamViec",
                table: "LichLamViecNhanViens");

            migrationBuilder.DropIndex(
                name: "IX_DangKyCaLamViecs_IdCaLamViec",
                table: "DangKyCaLamViecs");

            migrationBuilder.DropColumn(
                name: "IdCaLamViec",
                table: "LichLamViecNhanViens");

            migrationBuilder.DropColumn(
                name: "IdCaLamViec",
                table: "DangKyCaLamViecs");

            migrationBuilder.AddColumn<DateTime>(
                name: "GioKetThuc",
                table: "DangKyCaLamViecs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "GiobatDau",
                table: "DangKyCaLamViecs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayLam",
                table: "DangKyCaLamViecs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
