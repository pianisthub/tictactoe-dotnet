using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyAspNetCoreApp.Migrations
{
    /// <inheritdoc />
    public partial class AddGameIdAndPlayers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GameId",
                table: "PlayerScores",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GameId",
                table: "GameStates",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PlayerO",
                table: "GameStates",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PlayerX",
                table: "GameStates",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameId",
                table: "PlayerScores");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "GameStates");

            migrationBuilder.DropColumn(
                name: "PlayerO",
                table: "GameStates");

            migrationBuilder.DropColumn(
                name: "PlayerX",
                table: "GameStates");
        }
    }
}
