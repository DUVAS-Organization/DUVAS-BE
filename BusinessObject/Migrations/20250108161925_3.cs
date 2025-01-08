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
                name: "FK_Rooms_CategoryRooms_CategoryRoomId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "RoomCategory",
                table: "Rooms");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ServicePosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryRoomId",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Buildings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ServicePosts_UserId",
                table: "ServicePosts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_UserId",
                table: "Rooms",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_UserId",
                table: "Buildings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_Users_UserId",
                table: "Buildings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_CategoryRooms_CategoryRoomId",
                table: "Rooms",
                column: "CategoryRoomId",
                principalTable: "CategoryRooms",
                principalColumn: "CategoryRoomId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Users_UserId",
                table: "Rooms",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServicePosts_Users_UserId",
                table: "ServicePosts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_Users_UserId",
                table: "Buildings");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_CategoryRooms_CategoryRoomId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Users_UserId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_ServicePosts_Users_UserId",
                table: "ServicePosts");

            migrationBuilder.DropIndex(
                name: "IX_ServicePosts_UserId",
                table: "ServicePosts");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_UserId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Buildings_UserId",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ServicePosts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Buildings");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryRoomId",
                table: "Rooms",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "RoomCategory",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_CategoryRooms_CategoryRoomId",
                table: "Rooms",
                column: "CategoryRoomId",
                principalTable: "CategoryRooms",
                principalColumn: "CategoryRoomId");
        }
    }
}
