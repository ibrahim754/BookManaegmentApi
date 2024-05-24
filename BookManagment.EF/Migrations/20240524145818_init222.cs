using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookManagment.EF.Migrations
{
    /// <inheritdoc />
    public partial class init222 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rate_AspNetUsers_UserId",
                table: "AverageRate");

            migrationBuilder.DropForeignKey(
                name: "FK_Rate_Books_BookId",
                table: "AverageRate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rate",
                table: "AverageRate");

            migrationBuilder.RenameTable(
                name: "AverageRate",
                newName: "Rates");

            migrationBuilder.RenameIndex(
                name: "IX_Rate_UserId",
                table: "Rates",
                newName: "IX_Rates_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rates",
                table: "Rates",
                columns: new[] { "BookId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_AspNetUsers_UserId",
                table: "Rates",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_Books_BookId",
                table: "Rates",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rates_AspNetUsers_UserId",
                table: "Rates");

            migrationBuilder.DropForeignKey(
                name: "FK_Rates_Books_BookId",
                table: "Rates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rates",
                table: "Rates");

            migrationBuilder.RenameTable(
                name: "Rates",
                newName: "AverageRate");

            migrationBuilder.RenameIndex(
                name: "IX_Rates_UserId",
                table: "AverageRate",
                newName: "IX_Rate_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rate",
                table: "AverageRate",
                columns: new[] { "BookId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Rate_AspNetUsers_UserId",
                table: "AverageRate",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rate_Books_BookId",
                table: "AverageRate",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
