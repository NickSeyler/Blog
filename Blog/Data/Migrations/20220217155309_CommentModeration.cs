using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Data.Migrations
{
    public partial class CommentModeration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModeratedBody",
                table: "Comment",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModeratedDate",
                table: "Comment",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModerationReason",
                table: "Comment",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ModeratorId",
                table: "Comment",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ModeratorId",
                table: "Comment",
                column: "ModeratorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AspNetUsers_ModeratorId",
                table: "Comment",
                column: "ModeratorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AspNetUsers_ModeratorId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_ModeratorId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "ModeratedBody",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "ModeratedDate",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "ModerationReason",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "ModeratorId",
                table: "Comment");
        }
    }
}
