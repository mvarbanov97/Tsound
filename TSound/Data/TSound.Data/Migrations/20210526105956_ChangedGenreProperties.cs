using Microsoft.EntityFrameworkCore.Migrations;

namespace TSound.Data.Migrations
{
    public partial class ChangedGenreProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeezerId",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "PictureURL",
                table: "Genres");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Genres",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Genres",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpotifyId",
                table: "Genres",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "SpotifyId",
                table: "Genres");

            migrationBuilder.AddColumn<string>(
                name: "DeezerId",
                table: "Genres",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PictureURL",
                table: "Genres",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
