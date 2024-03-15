using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkBuddy.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RenameTableClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatBoxes_Clients_GroupCreatorId",
                table: "ChatBoxes");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientChatBoxes_Clients_ClientId",
                table: "ClientChatBoxes");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientMessages_Clients_ClientId",
                table: "ClientMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Clients_ReceiverId",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Clients_SenderID",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Clients_SenderId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Clients_ClientId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_OtpCodes_Clients_ClientId",
                table: "OtpCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Clients_InformantClientId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Clients_ReportedClientId",
                table: "Reports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clients",
                table: "Clients");

            migrationBuilder.RenameTable(
                name: "Clients",
                newName: "Users");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatBoxes_Users_GroupCreatorId",
                table: "ChatBoxes",
                column: "GroupCreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientChatBoxes_Users_ClientId",
                table: "ClientChatBoxes",
                column: "ClientId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientMessages_Users_ClientId",
                table: "ClientMessages",
                column: "ClientId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_ReceiverId",
                table: "Friendships",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_SenderID",
                table: "Friendships",
                column: "SenderID",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Users_ClientId",
                table: "Notification",
                column: "ClientId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OtpCodes_Users_ClientId",
                table: "OtpCodes",
                column: "ClientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_InformantClientId",
                table: "Reports",
                column: "InformantClientId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_ReportedClientId",
                table: "Reports",
                column: "ReportedClientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatBoxes_Users_GroupCreatorId",
                table: "ChatBoxes");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientChatBoxes_Users_ClientId",
                table: "ClientChatBoxes");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientMessages_Users_ClientId",
                table: "ClientMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_ReceiverId",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_SenderID",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_SenderId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Users_ClientId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_OtpCodes_Users_ClientId",
                table: "OtpCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_InformantClientId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_ReportedClientId",
                table: "Reports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Clients");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clients",
                table: "Clients",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatBoxes_Clients_GroupCreatorId",
                table: "ChatBoxes",
                column: "GroupCreatorId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientChatBoxes_Clients_ClientId",
                table: "ClientChatBoxes",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientMessages_Clients_ClientId",
                table: "ClientMessages",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Clients_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Clients_ClientId",
                table: "Notification",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OtpCodes_Clients_ClientId",
                table: "OtpCodes",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Clients_InformantClientId",
                table: "Reports",
                column: "InformantClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Clients_ReportedClientId",
                table: "Reports",
                column: "ReportedClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
