using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web3Multitool.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddedUsdtColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "UsdtBalance",
                table: "AddressChainInfos",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsdtBalance",
                table: "AddressChainInfos");
        }
    }
}
