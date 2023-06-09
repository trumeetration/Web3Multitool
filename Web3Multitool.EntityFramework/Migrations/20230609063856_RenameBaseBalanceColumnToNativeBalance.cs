using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web3Multitool.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class RenameBaseBalanceColumnToNativeBalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BaseBalance",
                table: "AddressChainInfos",
                newName: "NativeBalance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NativeBalance",
                table: "AddressChainInfos",
                newName: "BaseBalance");
        }
    }
}
