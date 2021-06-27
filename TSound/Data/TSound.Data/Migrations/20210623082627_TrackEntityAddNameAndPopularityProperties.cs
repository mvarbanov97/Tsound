using Microsoft.EntityFrameworkCore.Migrations;

namespace TSound.Data.Migrations
{
    public partial class TrackEntityAddNameAndPopularityProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rank",
                table: "Tracks");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Tracks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Popularity",
                table: "Tracks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "Popularity",
                table: "Tracks");

            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "Tracks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
