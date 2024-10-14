using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookHub.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedFriendship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersAchievments_Achievments_AchievmentsId",
                table: "UsersAchievments");

            migrationBuilder.DropTable(
                name: "Achievments");

            migrationBuilder.CreateTable(
                name: "Achievements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievements", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_UsersAchievments_Achievements_AchievmentsId",
                table: "UsersAchievments",
                column: "AchievmentsId",
                principalTable: "Achievements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersAchievments_Achievements_AchievmentsId",
                table: "UsersAchievments");

            migrationBuilder.DropTable(
                name: "Achievements");

            migrationBuilder.CreateTable(
                name: "Achievments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievments", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_UsersAchievments_Achievments_AchievmentsId",
                table: "UsersAchievments",
                column: "AchievmentsId",
                principalTable: "Achievments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
