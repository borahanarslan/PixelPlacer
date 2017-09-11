using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PixelPlacer.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Video_Project_ProjectId",
                table: "Video");

            migrationBuilder.DropIndex(
                name: "IX_Video_ProjectId",
                table: "Video");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Video");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Video",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Video_ProjectId",
                table: "Video",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Video_Project_ProjectId",
                table: "Video",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
