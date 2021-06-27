using Microsoft.EntityFrameworkCore.Migrations;

namespace TSound.Data.Migrations
{
    public partial class AddSpotifyIdProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SpotifyId",
                table: "Playlists",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpotifyId",
                table: "Playlists");
        }
    }
}
