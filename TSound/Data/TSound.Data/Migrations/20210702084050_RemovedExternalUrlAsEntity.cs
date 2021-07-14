using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TSound.Data.Migrations
{
    public partial class RemovedExternalUrlAsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_ExternalUrls_ExternalUrlsId",
                table: "Albums");

            migrationBuilder.DropForeignKey(
                name: "FK_Artists_ExternalUrls_ExternalUrlsId",
                table: "Artists");

            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_ExternalUrls_ExternalUrlsId",
                table: "Tracks");

            migrationBuilder.DropTable(
                name: "ExternalUrls");

            migrationBuilder.DropIndex(
                name: "IX_Tracks_ExternalUrlsId",
                table: "Tracks");

            migrationBuilder.DropIndex(
                name: "IX_Artists_ExternalUrlsId",
                table: "Artists");

            migrationBuilder.DropIndex(
                name: "IX_Albums_ExternalUrlsId",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "ExternalUrlsId",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "ExternalUrlsId",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "ExternalUrlsId",
                table: "Albums");

            migrationBuilder.AddColumn<string>(
                name: "ExternalUrls",
                table: "Tracks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalUrls",
                table: "Artists",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalUrls",
                table: "Albums",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalUrls",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "ExternalUrls",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "ExternalUrls",
                table: "Albums");

            migrationBuilder.AddColumn<Guid>(
                name: "ExternalUrlsId",
                table: "Tracks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExternalUrlsId",
                table: "Artists",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExternalUrlsId",
                table: "Albums",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExternalUrls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Spotify = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalUrls", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_ExternalUrlsId",
                table: "Tracks",
                column: "ExternalUrlsId");

            migrationBuilder.CreateIndex(
                name: "IX_Artists_ExternalUrlsId",
                table: "Artists",
                column: "ExternalUrlsId");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_ExternalUrlsId",
                table: "Albums",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_ExternalUrls_ExternalUrlsId",
                table: "Tracks",
                column: "ExternalUrlsId",
                principalTable: "ExternalUrls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
