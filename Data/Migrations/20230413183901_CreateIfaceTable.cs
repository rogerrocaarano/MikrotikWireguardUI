using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MikrotikWireguardUI.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateIfaceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Iface",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PrivateKey = table.Column<string>(type: "TEXT", nullable: true),
                    PublicKey = table.Column<string>(type: "TEXT", nullable: true),
                    ServerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Iface", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Iface_Server_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Server",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Iface_ServerId",
                table: "Iface",
                column: "ServerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Iface");
        }
    }
}
