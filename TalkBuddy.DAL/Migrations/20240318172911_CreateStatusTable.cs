using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkBuddy.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreateStatusTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientChatBoxStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientChatBoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientChatBoxStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientChatBoxStatus_ClientChatBoxes_ClientChatBoxId",
                        column: x => x.ClientChatBoxId,
                        principalTable: "ClientChatBoxes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClientChatBoxStatus_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientChatBoxStatus_ClientChatBoxId",
                table: "ClientChatBoxStatus",
                column: "ClientChatBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientChatBoxStatus_MessageId",
                table: "ClientChatBoxStatus",
                column: "MessageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientChatBoxStatus");
        }
    }
}
