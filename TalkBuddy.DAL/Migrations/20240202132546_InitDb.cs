using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkBuddy.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatBoxes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChatBoxName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    GroupCreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatBoxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatBoxes_Clients_GroupCreatorId",
                        column: x => x.GroupCreatorId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Friendships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Client1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Client2Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AcceptDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friendships_Clients_Client1Id",
                        column: x => x.Client1Id,
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Friendships_Clients_Client2Id",
                        column: x => x.Client2Id,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportedClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InformantClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_Clients_InformantClientId",
                        column: x => x.InformantClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_Clients_ReportedClientId",
                        column: x => x.ReportedClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdminChatBoxes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChatBoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminChatBoxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminChatBoxes_ChatBoxes_ChatBoxId",
                        column: x => x.ChatBoxId,
                        principalTable: "ChatBoxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdminChatBoxes_Clients_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClientChatBoxes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    IsLeft = table.Column<bool>(type: "bit", nullable: false),
                    IsNotificationOn = table.Column<bool>(type: "bit", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChatBoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientChatBoxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientChatBoxes_ChatBoxes_ChatBoxId",
                        column: x => x.ChatBoxId,
                        principalTable: "ChatBoxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientChatBoxes_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChatBoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_ChatBoxes_ChatBoxId",
                        column: x => x.ChatBoxId,
                        principalTable: "ChatBoxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeenDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientMessages_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClientMessages_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Medias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Capacity = table.Column<float>(type: "real", nullable: false),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medias_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaggedClientMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaggedClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaggedClientMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaggedClientMessages_Clients_TaggedClientId",
                        column: x => x.TaggedClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaggedClientMessages_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminChatBoxes_AdminId",
                table: "AdminChatBoxes",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminChatBoxes_ChatBoxId",
                table: "AdminChatBoxes",
                column: "ChatBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatBoxes_GroupCreatorId",
                table: "ChatBoxes",
                column: "GroupCreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientChatBoxes_ChatBoxId",
                table: "ClientChatBoxes",
                column: "ChatBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientChatBoxes_ClientId",
                table: "ClientChatBoxes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientMessages_ClientId",
                table: "ClientMessages",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientMessages_MessageId",
                table: "ClientMessages",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_Client1Id",
                table: "Friendships",
                column: "Client1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_Client2Id",
                table: "Friendships",
                column: "Client2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Medias_MessageId",
                table: "Medias",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatBoxId",
                table: "Messages",
                column: "ChatBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_InformantClientId",
                table: "Reports",
                column: "InformantClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportedClientId",
                table: "Reports",
                column: "ReportedClientId");

            migrationBuilder.CreateIndex(
                name: "IX_TaggedClientMessages_MessageId",
                table: "TaggedClientMessages",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_TaggedClientMessages_TaggedClientId",
                table: "TaggedClientMessages",
                column: "TaggedClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminChatBoxes");

            migrationBuilder.DropTable(
                name: "ClientChatBoxes");

            migrationBuilder.DropTable(
                name: "ClientMessages");

            migrationBuilder.DropTable(
                name: "Friendships");

            migrationBuilder.DropTable(
                name: "Medias");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "TaggedClientMessages");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "ChatBoxes");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
