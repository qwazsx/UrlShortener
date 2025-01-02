using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UrlInfos",
                columns: table => new
                {
                    RecordId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InsertDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LongUrl = table.Column<string>(type: "TEXT", nullable: false),
                    ShortUrl = table.Column<string>(type: "TEXT", nullable: false),
                    SpecifiedShortUrl = table.Column<string>(type: "TEXT", nullable: false),
                    IsShortUrlSpecified = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlInfos", x => x.RecordId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrlInfos");
        }
    }
}
