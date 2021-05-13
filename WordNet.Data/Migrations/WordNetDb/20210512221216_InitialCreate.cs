using Microsoft.EntityFrameworkCore.Migrations;

namespace WordNet.Data.Migrations.WordNetDb
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LexicalEntries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Lemma = table.Column<string>(type: "TEXT", nullable: true),
                    Language = table.Column<string>(type: "TEXT", nullable: true),
                    PartOfSpeech = table.Column<int>(type: "INTEGER", nullable: false),
                    Forms = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LexicalEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Synsets",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Ili = table.Column<string>(type: "TEXT", nullable: true),
                    PartOfSpeech = table.Column<int>(type: "INTEGER", nullable: false),
                    Definitions = table.Column<string>(type: "TEXT", nullable: true),
                    Examples = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Synsets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SyntacticBehaviours",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Senses = table.Column<string>(type: "TEXT", nullable: true),
                    SubcategorizationFrame = table.Column<string>(type: "TEXT", nullable: true),
                    LexicalEntryId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyntacticBehaviours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SyntacticBehaviours_LexicalEntries_LexicalEntryId",
                        column: x => x.LexicalEntryId,
                        principalTable: "LexicalEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Senses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    LexicalEntryId = table.Column<string>(type: "TEXT", nullable: true),
                    SynsetId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Senses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Senses_LexicalEntries_LexicalEntryId",
                        column: x => x.LexicalEntryId,
                        principalTable: "LexicalEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Senses_Synsets_SynsetId",
                        column: x => x.SynsetId,
                        principalTable: "Synsets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SynsetRelations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    SourceId = table.Column<string>(type: "TEXT", nullable: true),
                    TargetId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SynsetRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SynsetRelations_Synsets_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Synsets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SynsetRelations_Synsets_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Synsets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SenseRelations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    SourceId = table.Column<string>(type: "TEXT", nullable: true),
                    TargetId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SenseRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SenseRelations_Senses_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Senses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SenseRelations_Senses_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Senses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LexicalEntries_Lemma",
                table: "LexicalEntries",
                column: "Lemma");

            migrationBuilder.CreateIndex(
                name: "IX_SenseRelations_SourceId",
                table: "SenseRelations",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_SenseRelations_TargetId",
                table: "SenseRelations",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_Senses_LexicalEntryId",
                table: "Senses",
                column: "LexicalEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Senses_SynsetId",
                table: "Senses",
                column: "SynsetId");

            migrationBuilder.CreateIndex(
                name: "IX_SynsetRelations_SourceId",
                table: "SynsetRelations",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_SynsetRelations_TargetId",
                table: "SynsetRelations",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_SyntacticBehaviours_LexicalEntryId",
                table: "SyntacticBehaviours",
                column: "LexicalEntryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SenseRelations");

            migrationBuilder.DropTable(
                name: "SynsetRelations");

            migrationBuilder.DropTable(
                name: "SyntacticBehaviours");

            migrationBuilder.DropTable(
                name: "Senses");

            migrationBuilder.DropTable(
                name: "LexicalEntries");

            migrationBuilder.DropTable(
                name: "Synsets");
        }
    }
}
