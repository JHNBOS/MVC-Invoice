using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InvoiceApplication.Data.Migrations
{
    public partial class _11april20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Logins",
                table: "Logins");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "Logins",
                column: "ID");

            migrationBuilder.RenameTable(
                name: "Logins",
                newName: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Logins",
                table: "User",
                column: "ID");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Logins");
        }
    }
}
