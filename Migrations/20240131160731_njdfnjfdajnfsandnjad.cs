using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iTerminal.Migrations
{
    public partial class njdfnjfdajnfsandnjad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Messages",
                type: "datetime(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Messages");
        }
    }
}
