using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web3Multitool.Migrations
{
    /// <inheritdoc />
    public partial class AddedAddressChainInfoToApplicationContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountInfos_AddressChainInfo_ArbitrumInfoId",
                table: "AccountInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountInfos_AddressChainInfo_AvaxInfoId",
                table: "AccountInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountInfos_AddressChainInfo_BscInfoId",
                table: "AccountInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountInfos_AddressChainInfo_FantomInfoId",
                table: "AccountInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountInfos_AddressChainInfo_PolygonInfoId",
                table: "AccountInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AddressChainInfo",
                table: "AddressChainInfo");

            migrationBuilder.RenameTable(
                name: "AddressChainInfo",
                newName: "AddressChainInfos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AddressChainInfos",
                table: "AddressChainInfos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountInfos_AddressChainInfos_ArbitrumInfoId",
                table: "AccountInfos",
                column: "ArbitrumInfoId",
                principalTable: "AddressChainInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountInfos_AddressChainInfos_AvaxInfoId",
                table: "AccountInfos",
                column: "AvaxInfoId",
                principalTable: "AddressChainInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountInfos_AddressChainInfos_BscInfoId",
                table: "AccountInfos",
                column: "BscInfoId",
                principalTable: "AddressChainInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountInfos_AddressChainInfos_FantomInfoId",
                table: "AccountInfos",
                column: "FantomInfoId",
                principalTable: "AddressChainInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountInfos_AddressChainInfos_PolygonInfoId",
                table: "AccountInfos",
                column: "PolygonInfoId",
                principalTable: "AddressChainInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountInfos_AddressChainInfos_ArbitrumInfoId",
                table: "AccountInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountInfos_AddressChainInfos_AvaxInfoId",
                table: "AccountInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountInfos_AddressChainInfos_BscInfoId",
                table: "AccountInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountInfos_AddressChainInfos_FantomInfoId",
                table: "AccountInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountInfos_AddressChainInfos_PolygonInfoId",
                table: "AccountInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AddressChainInfos",
                table: "AddressChainInfos");

            migrationBuilder.RenameTable(
                name: "AddressChainInfos",
                newName: "AddressChainInfo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AddressChainInfo",
                table: "AddressChainInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountInfos_AddressChainInfo_ArbitrumInfoId",
                table: "AccountInfos",
                column: "ArbitrumInfoId",
                principalTable: "AddressChainInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountInfos_AddressChainInfo_AvaxInfoId",
                table: "AccountInfos",
                column: "AvaxInfoId",
                principalTable: "AddressChainInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountInfos_AddressChainInfo_BscInfoId",
                table: "AccountInfos",
                column: "BscInfoId",
                principalTable: "AddressChainInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountInfos_AddressChainInfo_FantomInfoId",
                table: "AccountInfos",
                column: "FantomInfoId",
                principalTable: "AddressChainInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountInfos_AddressChainInfo_PolygonInfoId",
                table: "AccountInfos",
                column: "PolygonInfoId",
                principalTable: "AddressChainInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
