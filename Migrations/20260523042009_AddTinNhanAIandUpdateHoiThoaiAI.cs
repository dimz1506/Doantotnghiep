using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doantotnghiep.Migrations
{
    /// <inheritdoc />
    public partial class AddTinNhanAIandUpdateHoiThoaiAI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayCapNhatTriThuc",
                table: "TriThucs",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TriThucs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayXoaTriThuc",
                table: "TriThucs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TrangThaiTriThuc",
                table: "TriThucs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TuKhoa",
                table: "TriThucs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TinNhanAIs",
                columns: table => new
                {
                    IdTinNhanAI = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdHoiThoaiAI = table.Column<int>(type: "int", nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    thoiGianGui = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TinNhanAIs", x => x.IdTinNhanAI);
                    table.ForeignKey(
                        name: "FK_TinNhanAIs_HoiThoaiAIs_IdHoiThoaiAI",
                        column: x => x.IdHoiThoaiAI,
                        principalTable: "HoiThoaiAIs",
                        principalColumn: "IdHoiThoaiAI",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TinNhanAIs_IdHoiThoaiAI",
                table: "TinNhanAIs",
                column: "IdHoiThoaiAI");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TinNhanAIs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TriThucs");

            migrationBuilder.DropColumn(
                name: "NgayXoaTriThuc",
                table: "TriThucs");

            migrationBuilder.DropColumn(
                name: "TrangThaiTriThuc",
                table: "TriThucs");

            migrationBuilder.DropColumn(
                name: "TuKhoa",
                table: "TriThucs");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayCapNhatTriThuc",
                table: "TriThucs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
