using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class _5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InsiderTradings_Users_UserId",
                table: "InsiderTradings");

            migrationBuilder.DropIndex(
                name: "IX_InsiderTradings_UserId",
                table: "InsiderTradings");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "InsiderTradings",
                newName: "Remitter");

            migrationBuilder.AddColumn<decimal>(
                name: "ChiPhiKhac",
                table: "Rooms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Dien",
                table: "Rooms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GuiXe",
                table: "Rooms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Internet",
                table: "Rooms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Nuoc",
                table: "Rooms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "QuanLy",
                table: "Rooms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Rac",
                table: "Rooms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HoldUntil",
                table: "InsiderTradings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Receiver",
                table: "InsiderTradings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReceiversUserId",
                table: "InsiderTradings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RemittersUserId",
                table: "InsiderTradings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "InsiderTradings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InsiderTradings_ReceiversUserId",
                table: "InsiderTradings",
                column: "ReceiversUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InsiderTradings_RemittersUserId",
                table: "InsiderTradings",
                column: "RemittersUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InsiderTradings_Users_ReceiversUserId",
                table: "InsiderTradings",
                column: "ReceiversUserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InsiderTradings_Users_RemittersUserId",
                table: "InsiderTradings",
                column: "RemittersUserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InsiderTradings_Users_ReceiversUserId",
                table: "InsiderTradings");

            migrationBuilder.DropForeignKey(
                name: "FK_InsiderTradings_Users_RemittersUserId",
                table: "InsiderTradings");

            migrationBuilder.DropIndex(
                name: "IX_InsiderTradings_ReceiversUserId",
                table: "InsiderTradings");

            migrationBuilder.DropIndex(
                name: "IX_InsiderTradings_RemittersUserId",
                table: "InsiderTradings");

            migrationBuilder.DropColumn(
                name: "ChiPhiKhac",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Dien",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "GuiXe",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Internet",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Nuoc",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "QuanLy",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Rac",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "HoldUntil",
                table: "InsiderTradings");

            migrationBuilder.DropColumn(
                name: "Receiver",
                table: "InsiderTradings");

            migrationBuilder.DropColumn(
                name: "ReceiversUserId",
                table: "InsiderTradings");

            migrationBuilder.DropColumn(
                name: "RemittersUserId",
                table: "InsiderTradings");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "InsiderTradings");

            migrationBuilder.RenameColumn(
                name: "Remitter",
                table: "InsiderTradings",
                newName: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InsiderTradings_UserId",
                table: "InsiderTradings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InsiderTradings_Users_UserId",
                table: "InsiderTradings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
