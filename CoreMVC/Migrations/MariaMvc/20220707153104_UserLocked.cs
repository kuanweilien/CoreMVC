using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreMVC.Migrations.MariaMvc
{
    public partial class UserLocked : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Locked",
                table: "UserModel",
                type: "tinyint(1)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Locked",
                table: "UserModel");
        }
    }
}
