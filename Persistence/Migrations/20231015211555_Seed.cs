using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dogs",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TailLength = table.Column<double>(type: "float", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dogs", x => x.Name);
                });

            migrationBuilder.InsertData(
                table: "dogs",
                columns: new[] { "Name", "Color", "TailLength", "Weight" },
                values: new object[,]
                {
                    { "Bailey", "sable", 11.0, 25.0 },
                    { "Bella", "white", 10.0, 20.0 },
                    { "Buddy", "golden", 18.0, 45.0 },
                    { "Coco", "chocolate", 20.0, 40.0 },
                    { "Daisy", "fawn", 16.0, 35.0 },
                    { "Jessy", "black&white", 7.0, 14.0 },
                    { "Luna", "gray", 12.0, 28.0 },
                    { "Max", "spotted", 14.0, 30.0 },
                    { "Milo", "tan", 9.0, 18.0 },
                    { "Neo", "red&amber", 22.0, 32.0 },
                    { "Rocky", "brown", 15.0, 36.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dogs");
        }
    }
}
