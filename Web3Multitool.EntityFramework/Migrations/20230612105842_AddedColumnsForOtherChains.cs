using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web3Multitool.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddedColumnsForOtherChains : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BnbInfoId",
                table: "AccountInfo",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CoredaoInfoId",
                table: "AccountInfo",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "HarmonyInfoId",
                table: "AccountInfo",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AccountInfo_BnbInfoId",
                table: "AccountInfo",
                column: "BnbInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountInfo_CoredaoInfoId",
                table: "AccountInfo",
                column: "CoredaoInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountInfo_HarmonyInfoId",
                table: "AccountInfo",
                column: "HarmonyInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountInfo_AddressChainInfos_BnbInfoId",
                table: "AccountInfo",
                column: "BnbInfoId",
                principalTable: "AddressChainInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountInfo_AddressChainInfos_CoredaoInfoId",
                table: "AccountInfo",
                column: "CoredaoInfoId",
                principalTable: "AddressChainInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountInfo_AddressChainInfos_HarmonyInfoId",
                table: "AccountInfo",
                column: "HarmonyInfoId",
                principalTable: "AddressChainInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountInfo_AddressChainInfos_BnbInfoId",
                table: "AccountInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountInfo_AddressChainInfos_CoredaoInfoId",
                table: "AccountInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountInfo_AddressChainInfos_HarmonyInfoId",
                table: "AccountInfo");

            migrationBuilder.DropIndex(
                name: "IX_AccountInfo_BnbInfoId",
                table: "AccountInfo");

            migrationBuilder.DropIndex(
                name: "IX_AccountInfo_CoredaoInfoId",
                table: "AccountInfo");

            migrationBuilder.DropIndex(
                name: "IX_AccountInfo_HarmonyInfoId",
                table: "AccountInfo");

            migrationBuilder.DropColumn(
                name: "BnbInfoId",
                table: "AccountInfo");

            migrationBuilder.DropColumn(
                name: "CoredaoInfoId",
                table: "AccountInfo");

            migrationBuilder.DropColumn(
                name: "HarmonyInfoId",
                table: "AccountInfo");
        }
    }
}
