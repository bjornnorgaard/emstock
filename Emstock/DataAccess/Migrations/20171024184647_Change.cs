using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DataAccess.Migrations
{
    public partial class Change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Components_Types_TypeId",
                table: "Components");

            migrationBuilder.DropIndex(
                name: "IX_Components_TypeId",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "ComponentTypeId",
                table: "Components");

            migrationBuilder.AlterColumn<int>(
                name: "TypeId",
                table: "Components",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TypeId1",
                table: "Components",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Components_TypeId1",
                table: "Components",
                column: "TypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Components_Types_TypeId1",
                table: "Components",
                column: "TypeId1",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Components_Types_TypeId1",
                table: "Components");

            migrationBuilder.DropIndex(
                name: "IX_Components_TypeId1",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "TypeId1",
                table: "Components");

            migrationBuilder.AlterColumn<long>(
                name: "TypeId",
                table: "Components",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ComponentTypeId",
                table: "Components",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Components_TypeId",
                table: "Components",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Components_Types_TypeId",
                table: "Components",
                column: "TypeId",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
