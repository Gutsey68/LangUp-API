using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LangUp.Migrations
{
    /// <inheritdoc />
    public partial class Relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastReviewed",
                table: "Translations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepetitionLevel",
                table: "Translations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Translations_UserId",
                table: "Translations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Translations_Users_UserId",
                table: "Translations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Translations_Users_UserId",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_UserId",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "LastReviewed",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "RepetitionLevel",
                table: "Translations");
        }
    }
}
