using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TSound.Data.Migrations
{
    public partial class AddedArtistTableToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DeezerId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ArtistPageURL = table.Column<string>(nullable: false),
                    PictureURL = table.Column<string>(nullable: true),
                    AlbumCount = table.Column<int>(nullable: false),
                    FanCount = table.Column<int>(nullable: false),
                    SongListURL = table.Column<string>(nullable: false),
                    SongCount = table.Column<int>(nullable: true),
                    Age = table.Column<int>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Artists");
        }
    }
}
