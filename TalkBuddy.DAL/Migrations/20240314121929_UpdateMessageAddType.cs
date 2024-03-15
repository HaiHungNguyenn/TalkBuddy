using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkBuddy.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMessageAddType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        

            migrationBuilder.AddColumn<int>(
                name: "MessageType",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
          
        }
    }
}
