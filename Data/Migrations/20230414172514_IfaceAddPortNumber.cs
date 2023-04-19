using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MikrotikWireguardUI.Data.Migrations
{
    /// <inheritdoc />
    public partial class IfaceAddPortNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PortNumber",
                table: "Iface",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PortNumber",
                table: "Iface");
        }
    }
}
