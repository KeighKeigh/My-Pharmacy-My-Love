using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ELIXIR.DATA.Migrations
{
    public partial class OneChargingUpdateForOtherTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BusinessUnitCode",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessUnitName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentUnitCode",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentUnitName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OneChargingCode",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubUnitCode",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubUnitName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "deleted_at",
                table: "OneChargings",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "OneChargings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessUnitCode",
                table: "MoveOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessUnitName",
                table: "MoveOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentUnitCode",
                table: "MoveOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentUnitName",
                table: "MoveOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OneChargingCode",
                table: "MoveOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OneChargingName",
                table: "MoveOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubUnitCode",
                table: "MoveOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubUnitName",
                table: "MoveOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountCode",
                table: "MiscellaneousReceipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountTitle",
                table: "MiscellaneousReceipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessUnitCode",
                table: "MiscellaneousReceipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessUnitName",
                table: "MiscellaneousReceipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyCode",
                table: "MiscellaneousReceipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "MiscellaneousReceipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentCode",
                table: "MiscellaneousReceipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentName",
                table: "MiscellaneousReceipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentUnitCode",
                table: "MiscellaneousReceipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentUnitName",
                table: "MiscellaneousReceipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationCode",
                table: "MiscellaneousReceipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationName",
                table: "MiscellaneousReceipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OneChargingCode",
                table: "MiscellaneousReceipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OneChargingName",
                table: "MiscellaneousReceipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubUnitCode",
                table: "MiscellaneousReceipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubUnitName",
                table: "MiscellaneousReceipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountCode",
                table: "MiscellaneousIssues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountTitle",
                table: "MiscellaneousIssues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessUnitCode",
                table: "MiscellaneousIssues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessUnitName",
                table: "MiscellaneousIssues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyCode",
                table: "MiscellaneousIssues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "MiscellaneousIssues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentCode",
                table: "MiscellaneousIssues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentName",
                table: "MiscellaneousIssues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentUnitCode",
                table: "MiscellaneousIssues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentUnitName",
                table: "MiscellaneousIssues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationCode",
                table: "MiscellaneousIssues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationName",
                table: "MiscellaneousIssues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OneChargingCode",
                table: "MiscellaneousIssues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OneChargingName",
                table: "MiscellaneousIssues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubUnitCode",
                table: "MiscellaneousIssues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubUnitName",
                table: "MiscellaneousIssues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OneAccountTitles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountTitleCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountTitleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SyncId = table.Column<int>(type: "int", nullable: true),
                    Delete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneAccountTitles", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OneAccountTitles");

            migrationBuilder.DropColumn(
                name: "BusinessUnitCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BusinessUnitName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DepartmentUnitCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DepartmentUnitName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OneChargingCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SubUnitCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SubUnitName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "OneChargings");

            migrationBuilder.DropColumn(
                name: "BusinessUnitCode",
                table: "MoveOrders");

            migrationBuilder.DropColumn(
                name: "BusinessUnitName",
                table: "MoveOrders");

            migrationBuilder.DropColumn(
                name: "DepartmentUnitCode",
                table: "MoveOrders");

            migrationBuilder.DropColumn(
                name: "DepartmentUnitName",
                table: "MoveOrders");

            migrationBuilder.DropColumn(
                name: "OneChargingCode",
                table: "MoveOrders");

            migrationBuilder.DropColumn(
                name: "OneChargingName",
                table: "MoveOrders");

            migrationBuilder.DropColumn(
                name: "SubUnitCode",
                table: "MoveOrders");

            migrationBuilder.DropColumn(
                name: "SubUnitName",
                table: "MoveOrders");

            migrationBuilder.DropColumn(
                name: "AccountCode",
                table: "MiscellaneousReceipts");

            migrationBuilder.DropColumn(
                name: "AccountTitle",
                table: "MiscellaneousReceipts");

            migrationBuilder.DropColumn(
                name: "BusinessUnitCode",
                table: "MiscellaneousReceipts");

            migrationBuilder.DropColumn(
                name: "BusinessUnitName",
                table: "MiscellaneousReceipts");

            migrationBuilder.DropColumn(
                name: "CompanyCode",
                table: "MiscellaneousReceipts");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "MiscellaneousReceipts");

            migrationBuilder.DropColumn(
                name: "DepartmentCode",
                table: "MiscellaneousReceipts");

            migrationBuilder.DropColumn(
                name: "DepartmentName",
                table: "MiscellaneousReceipts");

            migrationBuilder.DropColumn(
                name: "DepartmentUnitCode",
                table: "MiscellaneousReceipts");

            migrationBuilder.DropColumn(
                name: "DepartmentUnitName",
                table: "MiscellaneousReceipts");

            migrationBuilder.DropColumn(
                name: "LocationCode",
                table: "MiscellaneousReceipts");

            migrationBuilder.DropColumn(
                name: "LocationName",
                table: "MiscellaneousReceipts");

            migrationBuilder.DropColumn(
                name: "OneChargingCode",
                table: "MiscellaneousReceipts");

            migrationBuilder.DropColumn(
                name: "OneChargingName",
                table: "MiscellaneousReceipts");

            migrationBuilder.DropColumn(
                name: "SubUnitCode",
                table: "MiscellaneousReceipts");

            migrationBuilder.DropColumn(
                name: "SubUnitName",
                table: "MiscellaneousReceipts");

            migrationBuilder.DropColumn(
                name: "AccountCode",
                table: "MiscellaneousIssues");

            migrationBuilder.DropColumn(
                name: "AccountTitle",
                table: "MiscellaneousIssues");

            migrationBuilder.DropColumn(
                name: "BusinessUnitCode",
                table: "MiscellaneousIssues");

            migrationBuilder.DropColumn(
                name: "BusinessUnitName",
                table: "MiscellaneousIssues");

            migrationBuilder.DropColumn(
                name: "CompanyCode",
                table: "MiscellaneousIssues");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "MiscellaneousIssues");

            migrationBuilder.DropColumn(
                name: "DepartmentCode",
                table: "MiscellaneousIssues");

            migrationBuilder.DropColumn(
                name: "DepartmentName",
                table: "MiscellaneousIssues");

            migrationBuilder.DropColumn(
                name: "DepartmentUnitCode",
                table: "MiscellaneousIssues");

            migrationBuilder.DropColumn(
                name: "DepartmentUnitName",
                table: "MiscellaneousIssues");

            migrationBuilder.DropColumn(
                name: "LocationCode",
                table: "MiscellaneousIssues");

            migrationBuilder.DropColumn(
                name: "LocationName",
                table: "MiscellaneousIssues");

            migrationBuilder.DropColumn(
                name: "OneChargingCode",
                table: "MiscellaneousIssues");

            migrationBuilder.DropColumn(
                name: "OneChargingName",
                table: "MiscellaneousIssues");

            migrationBuilder.DropColumn(
                name: "SubUnitCode",
                table: "MiscellaneousIssues");

            migrationBuilder.DropColumn(
                name: "SubUnitName",
                table: "MiscellaneousIssues");

            migrationBuilder.AlterColumn<string>(
                name: "deleted_at",
                table: "OneChargings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
