using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TSound.Data.Migrations
{
    public partial class AddedAdditionalColumnsToUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApiKey",
                schema: "dbo",
                table: "Users",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                schema: "dbo",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                schema: "dbo",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "dbo",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                schema: "dbo",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                schema: "dbo",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBanned",
                schema: "dbo",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "dbo",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "dbo",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiKey",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DateModified",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Image",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsBanned",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "dbo",
                table: "Users");
        }
    }
}
