using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doantotnghiep.Migrations
{
    /// <inheritdoc />
    public partial class updatecchitietdatlich : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDatLiches_DatLichs_IdDatLich",
                table: "ChiTietDatLiches");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDatLiches_DichVus_IdDichVu",
                table: "ChiTietDatLiches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChiTietDatLiches",
                table: "ChiTietDatLiches");

            migrationBuilder.RenameTable(
                name: "ChiTietDatLiches",
                newName: "ChiTietDatLichs");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietDatLiches_IdDichVu",
                table: "ChiTietDatLichs",
                newName: "IX_ChiTietDatLichs_IdDichVu");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietDatLiches_IdDatLich",
                table: "ChiTietDatLichs",
                newName: "IX_ChiTietDatLichs_IdDatLich");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChiTietDatLichs",
                table: "ChiTietDatLichs",
                column: "IdChiTietDatLich");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDatLichs_DatLichs_IdDatLich",
                table: "ChiTietDatLichs",
                column: "IdDatLich",
                principalTable: "DatLichs",
                principalColumn: "IdDatLich");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDatLichs_DichVus_IdDichVu",
                table: "ChiTietDatLichs",
                column: "IdDichVu",
                principalTable: "DichVus",
                principalColumn: "IdDichVu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDatLichs_DatLichs_IdDatLich",
                table: "ChiTietDatLichs");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDatLichs_DichVus_IdDichVu",
                table: "ChiTietDatLichs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChiTietDatLichs",
                table: "ChiTietDatLichs");

            migrationBuilder.RenameTable(
                name: "ChiTietDatLichs",
                newName: "ChiTietDatLiches");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietDatLichs_IdDichVu",
                table: "ChiTietDatLiches",
                newName: "IX_ChiTietDatLiches_IdDichVu");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietDatLichs_IdDatLich",
                table: "ChiTietDatLiches",
                newName: "IX_ChiTietDatLiches_IdDatLich");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChiTietDatLiches",
                table: "ChiTietDatLiches",
                column: "IdChiTietDatLich");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDatLiches_DatLichs_IdDatLich",
                table: "ChiTietDatLiches",
                column: "IdDatLich",
                principalTable: "DatLichs",
                principalColumn: "IdDatLich");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDatLiches_DichVus_IdDichVu",
                table: "ChiTietDatLiches",
                column: "IdDichVu",
                principalTable: "DichVus",
                principalColumn: "IdDichVu");
        }
    }
}
