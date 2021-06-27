using Microsoft.EntityFrameworkCore.Migrations;

namespace TSound.Data.Migrations
{
    public partial class AddedSongsCountToPlaylist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SongsCount",
                table: "Playlists",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SongsCount",
                table: "Playlists");
        }
    }
}
