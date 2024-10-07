using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookHub.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Test4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Users_User1Id",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Users_User2Id",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_FriendId",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_UserId",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersAchievments_Achievements_AchievmentsId",
                table: "UsersAchievments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friendships",
                table: "Friendships");

            migrationBuilder.DropIndex(
                name: "IX_Friendships_User2Id",
                table: "Friendships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Achievements",
                table: "Achievements");

            migrationBuilder.DropColumn(
                name: "User1Id",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "User2Id",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "User1Id",
                table: "Friendships");

            migrationBuilder.RenameTable(
                name: "Friendships",
                newName: "FriendshipEntity");

            migrationBuilder.RenameTable(
                name: "Achievements",
                newName: "Achievments");

            migrationBuilder.RenameColumn(
                name: "User2Id",
                table: "FriendshipEntity",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "FriendshipEntity",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FriendId",
                table: "FriendshipEntity",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "FriendshipEntity",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_FriendshipEntity",
                table: "FriendshipEntity",
                columns: new[] { "FriendId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Achievments",
                table: "Achievments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FriendshipEntity_UserId",
                table: "FriendshipEntity",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendshipEntity_Users_FriendId",
                table: "FriendshipEntity",
                column: "FriendId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendshipEntity_Users_UserId",
                table: "FriendshipEntity",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersAchievments_Achievments_AchievmentsId",
                table: "UsersAchievments",
                column: "AchievmentsId",
                principalTable: "Achievments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendshipEntity_Users_FriendId",
                table: "FriendshipEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendshipEntity_Users_UserId",
                table: "FriendshipEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersAchievments_Achievments_AchievmentsId",
                table: "UsersAchievments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FriendshipEntity",
                table: "FriendshipEntity");

            migrationBuilder.DropIndex(
                name: "IX_FriendshipEntity_UserId",
                table: "FriendshipEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Achievments",
                table: "Achievments");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "FriendshipEntity");

            migrationBuilder.RenameTable(
                name: "FriendshipEntity",
                newName: "Friendships");

            migrationBuilder.RenameTable(
                name: "Achievments",
                newName: "Achievements");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Friendships",
                newName: "User2Id");

            migrationBuilder.AddColumn<int>(
                name: "User1Id",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "User2Id",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Friendships",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "FriendId",
                table: "Friendships",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "User1Id",
                table: "Friendships",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friendships",
                table: "Friendships",
                columns: new[] { "User1Id", "User2Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Achievements",
                table: "Achievements",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_User2Id",
                table: "Friendships",
                column: "User2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Users_User1Id",
                table: "Books",
                column: "User1Id",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Users_User2Id",
                table: "Books",
                column: "User2Id",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_FriendId",
                table: "Friendships",
                column: "FriendId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_UserId",
                table: "Friendships",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersAchievments_Achievements_AchievmentsId",
                table: "UsersAchievments",
                column: "AchievmentsId",
                principalTable: "Achievements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
