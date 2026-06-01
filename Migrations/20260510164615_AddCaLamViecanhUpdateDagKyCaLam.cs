using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doantotnghiep.Migrations
{
    /// <inheritdoc />
    public partial class AddCaLamViecanhUpdateDagKyCaLam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TrangThaiLichLamViecNV",
                table: "LichLamViecNhanViens",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateTable(
                name: "DangKyCaLamViecs",
                columns: table => new
                {
                    IdDangKyca = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdNhanVien = table.Column<int>(type: "int", nullable: false),
                    NgayLam = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GiobatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DangKyCaLamViecs", x => x.IdDangKyca);
                    table.ForeignKey(
                        name: "FK_DangKyCaLamViecs_NhanViens_IdNhanVien",
                        column: x => x.IdNhanVien,
                        principalTable: "NhanViens",
                        principalColumn: "IdNhanVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DangKyCaLamViecs_IdNhanVien",
                table: "DangKyCaLamViecs",
                column: "IdNhanVien");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DangKyCaLamViecs");

            migrationBuilder.AlterColumn<bool>(
                name: "TrangThaiLichLamViecNV",
                table: "LichLamViecNhanViens",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
