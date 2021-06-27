using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TSound.Data.Migrations
{
    public partial class ChangedDomainModelsToWorkWithSpotifyApi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Artists_ArtistId",
                table: "Albums");

            migrationBuilder.DropTable(
                name: "PlaylistsGenres");

            migrationBuilder.DropTable(
                name: "PlaylistSongs");

            migrationBuilder.DropTable(
                name: "Songs");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropIndex(
                name: "IX_Albums_ArtistId",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "AlbumCount",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "ArtistPageURL",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "DeezerId",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "FanCount",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "PictureURL",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "SongListURL",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "ArtistId",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "DeezerId",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "SongCount",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "SonglistUrl",
                table: "Albums");

            migrationBuilder.AddColumn<bool>(
                name: "Collaborative",
                table: "Playlists",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Uri",
                table: "Playlists",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExternalUrlsId",
                table: "Artists",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpotifyId",
                table: "Artists",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Uri",
                table: "Artists",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AlbumType",
                table: "Albums",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExternalUrlsId",
                table: "Albums",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReleaseDate",
                table: "Albums",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpotifyId",
                table: "Albums",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "TotalTracks",
                table: "Albums",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Uri",
                table: "Albums",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AlbumArtists",
                columns: table => new
                {
                    AlbumId = table.Column<Guid>(nullable: false),
                    ArtistId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumArtists", x => new { x.AlbumId, x.ArtistId });
                    table.ForeignKey(
                        name: "FK_AlbumArtists_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlbumArtists_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SpotifyId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Link = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    SongCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExternalUrls",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Spotify = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistsCategories",
                columns: table => new
                {
                    PlaylistId = table.Column<Guid>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistsCategories", x => new { x.PlaylistId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_PlaylistsCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlaylistsCategories_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SpotifyId = table.Column<string>(nullable: false),
                    DurationMs = table.Column<int>(nullable: false),
                    Explicit = table.Column<bool>(nullable: false),
                    ExternalUrlsId = table.Column<Guid>(nullable: true),
                    Rank = table.Column<int>(nullable: false),
                    ReleaseDate = table.Column<DateTime>(nullable: false),
                    PreviewUrl = table.Column<string>(nullable: true),
                    Uri = table.Column<string>(nullable: true),
                    AlbumId = table.Column<Guid>(nullable: false),
                    ArtistId = table.Column<Guid>(nullable: false),
                    SpotifyCategoryId = table.Column<string>(nullable: true),
                    CategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tracks_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tracks_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tracks_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tracks_ExternalUrls_ExternalUrlsId",
                        column: x => x.ExternalUrlsId,
                        principalTable: "ExternalUrls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistTracks",
                columns: table => new
                {
                    PlaylistId = table.Column<Guid>(nullable: false),
                    TrackId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistTracks", x => new { x.PlaylistId, x.TrackId });
                    table.ForeignKey(
                        name: "FK_PlaylistTracks_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlaylistTracks_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Artists_ExternalUrlsId",
                table: "Artists",
                column: "ExternalUrlsId");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_ExternalUrlsId",
                table: "Albums",
                column: "ExternalUrlsId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumArtists_ArtistId",
                table: "AlbumArtists",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistsCategories_CategoryId",
                table: "PlaylistsCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistTracks_TrackId",
                table: "PlaylistTracks",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_AlbumId",
                table: "Tracks",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_ArtistId",
                table: "Tracks",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_CategoryId",
                table: "Tracks",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_ExternalUrlsId",
                table: "Tracks",
                column: "ExternalUrlsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_ExternalUrls_ExternalUrlsId",
                table: "Albums",
                column: "ExternalUrlsId",
                principalTable: "ExternalUrls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Artists_ExternalUrls_ExternalUrlsId",
                table: "Artists",
                column: "ExternalUrlsId",
                principalTable: "ExternalUrls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_ExternalUrls_ExternalUrlsId",
                table: "Albums");

            migrationBuilder.DropForeignKey(
                name: "FK_Artists_ExternalUrls_ExternalUrlsId",
                table: "Artists");

            migrationBuilder.DropTable(
                name: "AlbumArtists");

            migrationBuilder.DropTable(
                name: "PlaylistsCategories");

            migrationBuilder.DropTable(
                name: "PlaylistTracks");

            migrationBuilder.DropTable(
                name: "Tracks");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "ExternalUrls");

            migrationBuilder.DropIndex(
                name: "IX_Artists_ExternalUrlsId",
                table: "Artists");

            migrationBuilder.DropIndex(
                name: "IX_Albums_ExternalUrlsId",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "Collaborative",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "Uri",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "ExternalUrlsId",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "SpotifyId",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "Uri",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "AlbumType",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "ExternalUrlsId",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "SpotifyId",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "TotalTracks",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "Uri",
                table: "Albums");

            migrationBuilder.AddColumn<int>(
                name: "AlbumCount",
                table: "Artists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ArtistPageURL",
                table: "Artists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeezerId",
                table: "Artists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FanCount",
                table: "Artists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PictureURL",
                table: "Artists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SongListURL",
                table: "Artists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ArtistId",
                table: "Albums",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "DeezerId",
                table: "Albums",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SongCount",
                table: "Albums",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SonglistUrl",
                table: "Albums",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SongCount = table.Column<int>(type: "int", nullable: false),
                    SpotifyId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistsGenres",
                columns: table => new
                {
                    PlaylistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistsGenres", x => new { x.PlaylistId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_PlaylistsGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlaylistsGenres_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AlbumId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeezerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreviewURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SongURL = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Songs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Songs_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Songs_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Songs_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistSongs",
                columns: table => new
                {
                    PlaylistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SongId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistSongs", x => new { x.PlaylistId, x.SongId });
                    table.ForeignKey(
                        name: "FK_PlaylistSongs_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlaylistSongs_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Albums_ArtistId",
                table: "Albums",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistsGenres_GenreId",
                table: "PlaylistsGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistSongs_SongId",
                table: "PlaylistSongs",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_AlbumId",
                table: "Songs",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_ArtistId",
                table: "Songs",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_GenreId",
                table: "Songs",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Artists_ArtistId",
                table: "Albums",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
