using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkBuddy.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddIsVerifiedToClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Clients",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Clients");
        }
    }
}
