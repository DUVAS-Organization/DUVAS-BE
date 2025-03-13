using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFieldWithdrawRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WithdrawRequests_TransactionId",
                table: "WithdrawRequests");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionId",
                table: "WithdrawRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawRequests_TransactionId",
                table: "WithdrawRequests",
                column: "TransactionId",
                unique: true,
                filter: "[TransactionId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WithdrawRequests_TransactionId",
                table: "WithdrawRequests");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionId",
                table: "WithdrawRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawRequests_TransactionId",
                table: "WithdrawRequests",
                column: "TransactionId",
                unique: true);
        }
    }
}
