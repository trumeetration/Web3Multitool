using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web3Multitool.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddressChainInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChainId = table.Column<int>(type: "INTEGER", nullable: false),
                    TxAmount = table.Column<int>(type: "INTEGER", nullable: false),
                    BaseBalance = table.Column<double>(type: "REAL", nullable: false),
                    FirstTxDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UsdtBalance = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressChainInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    CexAddress = table.Column<string>(type: "TEXT", nullable: false),
                    PrivateKey = table.Column<string>(type: "TEXT", nullable: false),
                    BscInfoId = table.Column<int>(type: "INTEGER", nullable: false),
                    AvaxInfoId = table.Column<int>(type: "INTEGER", nullable: false),
                    PolygonInfoId = table.Column<int>(type: "INTEGER", nullable: false),
                    ArbitrumInfoId = table.Column<int>(type: "INTEGER", nullable: false),
                    FantomInfoId = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalBalanceUsd = table.Column<double>(type: "REAL", nullable: false),
                    TotalTxAmount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountInfos_AddressChainInfo_ArbitrumInfoId",
                        column: x => x.ArbitrumInfoId,
                        principalTable: "AddressChainInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountInfos_AddressChainInfo_AvaxInfoId",
                        column: x => x.AvaxInfoId,
                        principalTable: "AddressChainInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountInfos_AddressChainInfo_BscInfoId",
                        column: x => x.BscInfoId,
                        principalTable: "AddressChainInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountInfos_AddressChainInfo_FantomInfoId",
                        column: x => x.FantomInfoId,
                        principalTable: "AddressChainInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountInfos_AddressChainInfo_PolygonInfoId",
                        column: x => x.PolygonInfoId,
                        principalTable: "AddressChainInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountInfos_ArbitrumInfoId",
                table: "AccountInfos",
                column: "ArbitrumInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountInfos_AvaxInfoId",
                table: "AccountInfos",
                column: "AvaxInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountInfos_BscInfoId",
                table: "AccountInfos",
                column: "BscInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountInfos_FantomInfoId",
                table: "AccountInfos",
                column: "FantomInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountInfos_PolygonInfoId",
                table: "AccountInfos",
                column: "PolygonInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountInfos");

            migrationBuilder.DropTable(
                name: "AddressChainInfo");
        }
    }
}
