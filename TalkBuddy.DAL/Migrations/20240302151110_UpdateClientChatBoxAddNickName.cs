using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkBuddy.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClientChatBoxAddNickName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "ClientChatBoxes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NickName",
                table: "ClientChatBoxes");
        }
    }
}
