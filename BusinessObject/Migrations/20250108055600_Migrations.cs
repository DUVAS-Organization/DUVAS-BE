using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class Migrations : Migration
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

            migrationBuilder.AlterColumn<int>(
                name: "CategoryRoomId",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_CategoryRooms_CategoryRoomId",
                table: "Rooms",
                column: "CategoryRoomId",
                principalTable: "CategoryRooms",
                principalColumn: "CategoryRoomId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_CategoryRooms_CategoryRoomId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Feedback",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Reports");

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
