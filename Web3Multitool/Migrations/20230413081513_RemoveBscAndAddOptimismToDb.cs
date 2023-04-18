using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web3Multitool.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBscAndAddOptimismToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountInfos_AddressChainInfos_BscInfoId",
                table: "AccountInfos");

            migrationBuilder.RenameColumn(
                name: "UsdtBalance",
                table: "AddressChainInfos",
                newName: "UsdcBalance");

            migrationBuilder.RenameColumn(
                name: "BscInfoId",
                table: "AccountInfos",
                newName: "OptimismInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountInfos_BscInfoId",
                table: "AccountInfos",
                newName: "IX_AccountInfos_OptimismInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountInfos_AddressChainInfos_OptimismInfoId",
                table: "AccountInfos",
                column: "OptimismInfoId",
                principalTable: "AddressChainInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountInfos_AddressChainInfos_OptimismInfoId",
                table: "AccountInfos");

            migrationBuilder.RenameColumn(
                name: "UsdcBalance",
                table: "AddressChainInfos",
                newName: "UsdtBalance");

            migrationBuilder.RenameColumn(
                name: "OptimismInfoId",
                table: "AccountInfos",
                newName: "BscInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountInfos_OptimismInfoId",
                table: "AccountInfos",
                newName: "IX_AccountInfos_BscInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountInfos_AddressChainInfos_BscInfoId",
                table: "AccountInfos",
                column: "BscInfoId",
                principalTable: "AddressChainInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
