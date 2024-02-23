using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkBuddy.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateFriendshipProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Clients_Client1Id",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Clients_Client2Id",
                table: "Friendships");

            migrationBuilder.RenameColumn(
                name: "Client2Id",
                table: "Friendships",
                newName: "SenderID");

            migrationBuilder.RenameColumn(
                name: "Client1Id",
                table: "Friendships",
                newName: "ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_Client2Id",
                table: "Friendships",
                newName: "IX_Friendships_SenderID");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_Client1Id",
                table: "Friendships",
                newName: "IX_Friendships_ReceiverId");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Friendships",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Clients_ReceiverId",
                table: "Friendships",
                column: "ReceiverId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Clients_SenderID",
                table: "Friendships",
                column: "SenderID",
                principalTable: "Clients",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Clients_ReceiverId",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Clients_SenderID",
                table: "Friendships");

            migrationBuilder.RenameColumn(
                name: "SenderID",
                table: "Friendships",
                newName: "Client2Id");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Friendships",
                newName: "Client1Id");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_SenderID",
                table: "Friendships",
                newName: "IX_Friendships_Client2Id");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_ReceiverId",
                table: "Friendships",
                newName: "IX_Friendships_Client1Id");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Friendships",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Clients_Client1Id",
                table: "Friendships",
                column: "Client1Id",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Clients_Client2Id",
                table: "Friendships",
                column: "Client2Id",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
