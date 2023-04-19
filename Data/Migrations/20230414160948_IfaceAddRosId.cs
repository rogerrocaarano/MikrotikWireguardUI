﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MikrotikWireguardUI.Data.Migrations
{
    /// <inheritdoc />
    public partial class IfaceAddRosId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RosId",
                table: "Iface",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RosId",
                table: "Iface");
        }
    }
}
