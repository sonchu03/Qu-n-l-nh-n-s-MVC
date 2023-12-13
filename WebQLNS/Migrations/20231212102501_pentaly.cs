using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebQLNS.Migrations
{
    /// <inheritdoc />
    public partial class pentaly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NhanVienId",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "NhanVienId",
                table: "Penalty");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NhanVienId",
                table: "Rewards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NhanVienId",
                table: "Penalty",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
