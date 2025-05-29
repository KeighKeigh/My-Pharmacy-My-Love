using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ELIXIR.DATA.Migrations
{
    public partial class NewTableOnecharging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OneChargings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sync_id = table.Column<int>(type: "int", nullable: true),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    company_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    company_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    company_id = table.Column<int>(type: "int", nullable: true),
                    business_unit_id = table.Column<int>(type: "int", nullable: true),
                    business_unit_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    business_unit_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    department_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    department_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    department_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    department_unit_id = table.Column<int>(type: "int", nullable: true),
                    department_unit_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    department_unit_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sub_unit_id = table.Column<int>(type: "int", nullable: true),
                    sub_unit_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sub_unit_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    location_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    location_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    location_id = table.Column<int>(type: "int", nullable: true),
                    deleted_at = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneChargings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OneChargings");
        }
    }
}
