using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KnowledgeBet.Infrastructure.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameUser_Games_GameId",
                table: "GameUser");

            migrationBuilder.DropForeignKey(
                name: "FK_GameUser_Users_UserId",
                table: "GameUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameUser",
                table: "GameUser");

            migrationBuilder.RenameTable(
                name: "GameUser",
                newName: "GamesByUser");

            migrationBuilder.RenameIndex(
                name: "IX_GameUser_UserId",
                table: "GamesByUser",
                newName: "IX_GamesByUser_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GamesByUser",
                table: "GamesByUser",
                columns: new[] { "GameId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GamesByUser_Games_GameId",
                table: "GamesByUser",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GamesByUser_Users_UserId",
                table: "GamesByUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamesByUser_Games_GameId",
                table: "GamesByUser");

            migrationBuilder.DropForeignKey(
                name: "FK_GamesByUser_Users_UserId",
                table: "GamesByUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GamesByUser",
                table: "GamesByUser");

            migrationBuilder.RenameTable(
                name: "GamesByUser",
                newName: "GameUser");

            migrationBuilder.RenameIndex(
                name: "IX_GamesByUser_UserId",
                table: "GameUser",
                newName: "IX_GameUser_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameUser",
                table: "GameUser",
                columns: new[] { "GameId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GameUser_Games_GameId",
                table: "GameUser",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameUser_Users_UserId",
                table: "GameUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
