using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WordNet.Data.Migrations
{
    public partial class AddLexicalEntryLastAccessed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastAccessed",
                table: "LexicalEntries",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastAccessed",
                table: "LexicalEntries");
        }
    }
}
