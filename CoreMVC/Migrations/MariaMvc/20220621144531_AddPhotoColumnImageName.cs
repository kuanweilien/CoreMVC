using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreMVC.Migrations.MariaMvc
{
    public partial class AddPhotoColumnImageName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "PhotoModel",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "PhotoModel");
        }
    }
}
