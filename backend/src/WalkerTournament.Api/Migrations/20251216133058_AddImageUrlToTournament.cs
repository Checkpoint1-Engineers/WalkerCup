using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WalkerTournament.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToTournament : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Tournaments",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Tournaments");
        }
    }
}
