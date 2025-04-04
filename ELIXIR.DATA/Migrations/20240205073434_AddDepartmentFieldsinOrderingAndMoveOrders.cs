using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ELIXIR.DATA.Migrations
{
    public partial class AddDepartmentFieldsinOrderingAndMoveOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountCode",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountTitle",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyCode",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentCode",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationCode",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountCode",
                table: "MoveOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountTitle",
                table: "MoveOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyCode",
                table: "MoveOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "MoveOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentCode",
                table: "MoveOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentName",
                table: "MoveOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationCode",
                table: "MoveOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationName",
                table: "MoveOrders",
                type: "nvarchar(max)",
                nullable: true);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "TransactionDate",
            //    table: "MiscellaneousReceipts",
            //    type: "datetime2",
            //    nullable: true);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "TransactionDate",
            //    table: "MiscellaneousIssues",
            //    type: "datetime2",
            //    nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AccountTitle",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CompanyCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DepartmentCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DepartmentName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "LocationCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "LocationName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AccountCode",
                table: "MoveOrders");

            migrationBuilder.DropColumn(
                name: "AccountTitle",
                table: "MoveOrders");

            migrationBuilder.DropColumn(
                name: "CompanyCode",
                table: "MoveOrders");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "MoveOrders");

            migrationBuilder.DropColumn(
                name: "DepartmentCode",
                table: "MoveOrders");

            migrationBuilder.DropColumn(
                name: "DepartmentName",
                table: "MoveOrders");

            migrationBuilder.DropColumn(
                name: "LocationCode",
                table: "MoveOrders");

            migrationBuilder.DropColumn(
                name: "LocationName",
                table: "MoveOrders");

            //migrationBuilder.DropColumn(
            //    name: "TransactionDate",
            //    table: "MiscellaneousReceipts");

            //migrationBuilder.DropColumn(
            //    name: "TransactionDate",
            //    table: "MiscellaneousIssues");
        }
    }
}
