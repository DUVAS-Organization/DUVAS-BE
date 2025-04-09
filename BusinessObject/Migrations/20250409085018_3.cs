using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class _3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_ServicePosts_ServicePostId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Transactions_TransactionId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ServicePostId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_TransactionId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ServicePostId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Reports");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServicePostId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ServicePostId",
                table: "Reports",
                column: "ServicePostId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_TransactionId",
                table: "Reports",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_ServicePosts_ServicePostId",
                table: "Reports",
                column: "ServicePostId",
                principalTable: "ServicePosts",
                principalColumn: "ServicePostId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Transactions_TransactionId",
                table: "Reports",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
