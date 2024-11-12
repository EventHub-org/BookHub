using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookHub.DAL.Migrations
{
    /// <inheritdoc />
    public partial class aaaaaaaaaaaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookEntityCollectionEntity",
                columns: table => new
                {
                    BooksId = table.Column<int>(type: "int", nullable: false),
                    CollectionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookEntityCollectionEntity", x => new { x.BooksId, x.CollectionsId });
                    table.ForeignKey(
                        name: "FK_BookEntityCollectionEntity_Books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookEntityCollectionEntity_Collections_CollectionsId",
                        column: x => x.CollectionsId,
                        principalTable: "Collections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookEntityCollectionEntity_CollectionsId",
                table: "BookEntityCollectionEntity",
                column: "CollectionsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookEntityCollectionEntity");
        }
    }
}
