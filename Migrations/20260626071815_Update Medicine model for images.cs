using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pharmacy.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMedicinemodelforimages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "imageSrc",
                table: "Medicines",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imageSrc",
                table: "Medicines");
        }
    }
}
