using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doantotnghiep.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatLichs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatLichs_ChiTietDatLiches_IdChiTietDatLich",
                table: "DatLichs");

            migrationBuilder.DropIndex(
                name: "IX_DatLichs_IdChiTietDatLich",
                table: "DatLichs");

            migrationBuilder.DropColumn(
                name: "IdChiTietDatLich",
                table: "DatLichs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdChiTietDatLich",
                table: "DatLichs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DatLichs_IdChiTietDatLich",
                table: "DatLichs",
                column: "IdChiTietDatLich");

            migrationBuilder.AddForeignKey(
                name: "FK_DatLichs_ChiTietDatLiches_IdChiTietDatLich",
                table: "DatLichs",
                column: "IdChiTietDatLich",
                principalTable: "ChiTietDatLiches",
                principalColumn: "IdChiTietDatLich",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
