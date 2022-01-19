﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KnowledgeBet.Infrastructure.Migrations
{
    public partial class newColumnInQuestionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Questions");
        }
    }
}
