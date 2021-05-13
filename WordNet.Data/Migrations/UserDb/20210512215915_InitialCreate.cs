using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WordNet.Data.Migrations.UserDb
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LexicalEntriesHistory",
                columns: table => new
                {
                    Lemma = table.Column<string>(type: "TEXT", nullable: false),
                    Language = table.Column<string>(type: "TEXT", nullable: false),
                    LastAccessed = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LexicalEntriesHistory", x => new { x.Lemma, x.Language });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LexicalEntriesHistory");
        }
    }
}
