using Microsoft.EntityFrameworkCore.Migrations;

namespace Badass.Core.Migrations
{
    public partial class addforeignkeytopost1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostTypeId",
                table: "Posts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PostTypeId",
                table: "Posts",
                column: "PostTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_PostTypes_PostTypeId",
                table: "Posts",
                column: "PostTypeId",
                principalTable: "PostTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_PostTypes_PostTypeId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_PostTypeId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PostTypeId",
                table: "Posts");
        }
    }
}
