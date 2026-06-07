using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doantotnghiep.Migrations
{
    /// <inheritdoc />
    public partial class deletecalamviec : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DangKyCaLamViecs");

            migrationBuilder.DropTable(
                name: "LichLamViecNhanViens");

            migrationBuilder.DropTable(
                name: "CaLamViecs");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "GioBatDauLamViec",
                table: "NhanViens",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "GioKetThucLamViec",
                table: "NhanViens",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<bool>(
                name: "LaNhanVienFullTime",
                table: "NhanViens",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GioBatDauLamViec",
                table: "NhanViens");

            migrationBuilder.DropColumn(
                name: "GioKetThucLamViec",
                table: "NhanViens");

            migrationBuilder.DropColumn(
                name: "LaNhanVienFullTime",
                table: "NhanViens");

            migrationBuilder.CreateTable(
                name: "CaLamViecs",
                columns: table => new
                {
                    IdCaLamViec = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GioBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MoDangKy = table.Column<bool>(type: "bit", nullable: false),
                    NgayLam = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoLuongNhanVienToiDa = table.Column<int>(type: "int", nullable: false),
                    TenCa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaLamViecs", x => x.IdCaLamViec);
                });

            migrationBuilder.CreateTable(
                name: "DangKyCaLamViecs",
                columns: table => new
                {
                    IdDangKyca = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCaLamViec = table.Column<int>(type: "int", nullable: false),
                    IdNhanVien = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DangKyCaLamViecs", x => x.IdDangKyca);
                    table.ForeignKey(
                        name: "FK_DangKyCaLamViecs_CaLamViecs_IdCaLamViec",
                        column: x => x.IdCaLamViec,
                        principalTable: "CaLamViecs",
                        principalColumn: "IdCaLamViec",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DangKyCaLamViecs_NhanViens_IdNhanVien",
                        column: x => x.IdNhanVien,
                        principalTable: "NhanViens",
                        principalColumn: "IdNhanVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LichLamViecNhanViens",
                columns: table => new
                {
                    IdLichLamViecNhanVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCaLamViec = table.Column<int>(type: "int", nullable: false),
                    IdNhanVien = table.Column<int>(type: "int", nullable: false),
                    GioBatDauCaLamViecNV = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioKetThucCaLamViecNV = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayLamViecNV = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThaiLichLamViecNV = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichLamViecNhanViens", x => x.IdLichLamViecNhanVien);
                    table.ForeignKey(
                        name: "FK_LichLamViecNhanViens_CaLamViecs_IdCaLamViec",
                        column: x => x.IdCaLamViec,
                        principalTable: "CaLamViecs",
                        principalColumn: "IdCaLamViec",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LichLamViecNhanViens_NhanViens_IdNhanVien",
                        column: x => x.IdNhanVien,
                        principalTable: "NhanViens",
                        principalColumn: "IdNhanVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DangKyCaLamViecs_IdCaLamViec",
                table: "DangKyCaLamViecs",
                column: "IdCaLamViec");

            migrationBuilder.CreateIndex(
                name: "IX_DangKyCaLamViecs_IdNhanVien",
                table: "DangKyCaLamViecs",
                column: "IdNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_LichLamViecNhanViens_IdCaLamViec",
                table: "LichLamViecNhanViens",
                column: "IdCaLamViec");

            migrationBuilder.CreateIndex(
                name: "IX_LichLamViecNhanViens_IdNhanVien",
                table: "LichLamViecNhanViens",
                column: "IdNhanVien");
        }
    }
}
