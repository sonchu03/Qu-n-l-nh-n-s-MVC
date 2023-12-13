using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebQLNS.Migrations
{
    /// <inheritdoc />
    public partial class Reward2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Penalty_NhanViens_NhanVienId",
                table: "Penalty");

            migrationBuilder.DropForeignKey(
                name: "FK_Rewards_NhanViens_NhanVienId",
                table: "Rewards");

            migrationBuilder.DropIndex(
                name: "IX_Rewards_NhanVienId",
                table: "Rewards");

            migrationBuilder.DropIndex(
                name: "IX_Penalty_NhanVienId",
                table: "Penalty");

            migrationBuilder.AddColumn<int>(
                name: "MaNhanVien",
                table: "Rewards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaNhanVien",
                table: "Penalty",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_MaNhanVien",
                table: "Rewards",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_Penalty_MaNhanVien",
                table: "Penalty",
                column: "MaNhanVien");

            migrationBuilder.AddForeignKey(
                name: "FK_Penalty_NhanViens_MaNhanVien",
                table: "Penalty",
                column: "MaNhanVien",
                principalTable: "NhanViens",
                principalColumn: "MaNhanVien",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rewards_NhanViens_MaNhanVien",
                table: "Rewards",
                column: "MaNhanVien",
                principalTable: "NhanViens",
                principalColumn: "MaNhanVien",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Penalty_NhanViens_MaNhanVien",
                table: "Penalty");

            migrationBuilder.DropForeignKey(
                name: "FK_Rewards_NhanViens_MaNhanVien",
                table: "Rewards");

            migrationBuilder.DropIndex(
                name: "IX_Rewards_MaNhanVien",
                table: "Rewards");

            migrationBuilder.DropIndex(
                name: "IX_Penalty_MaNhanVien",
                table: "Penalty");

            migrationBuilder.DropColumn(
                name: "MaNhanVien",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "MaNhanVien",
                table: "Penalty");

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_NhanVienId",
                table: "Rewards",
                column: "NhanVienId");

            migrationBuilder.CreateIndex(
                name: "IX_Penalty_NhanVienId",
                table: "Penalty",
                column: "NhanVienId");

            migrationBuilder.AddForeignKey(
                name: "FK_Penalty_NhanViens_NhanVienId",
                table: "Penalty",
                column: "NhanVienId",
                principalTable: "NhanViens",
                principalColumn: "MaNhanVien",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rewards_NhanViens_NhanVienId",
                table: "Rewards",
                column: "NhanVienId",
                principalTable: "NhanViens",
                principalColumn: "MaNhanVien",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
