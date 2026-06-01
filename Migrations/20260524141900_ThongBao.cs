using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doantotnghiep.Migrations
{
    /// <inheritdoc />
    public partial class ThongBao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ThongBaos",
                columns: table => new
                {
                    IdThongBao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdKhachHang = table.Column<int>(type: "int", nullable: true),
                    IdNhanVien = table.Column<int>(type: "int", nullable: true),
                    TieuDe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DuongDan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DaDoc = table.Column<bool>(type: "bit", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaos", x => x.IdThongBao);
                    table.ForeignKey(
                        name: "FK_ThongBaos_KhachHangs_IdKhachHang",
                        column: x => x.IdKhachHang,
                        principalTable: "KhachHangs",
                        principalColumn: "IdKhachHang");
                    table.ForeignKey(
                        name: "FK_ThongBaos_NhanViens_IdNhanVien",
                        column: x => x.IdNhanVien,
                        principalTable: "NhanViens",
                        principalColumn: "IdNhanVien");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaos_IdKhachHang",
                table: "ThongBaos",
                column: "IdKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaos_IdNhanVien",
                table: "ThongBaos",
                column: "IdNhanVien");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThongBaos");
        }
    }
}
