using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web3Multitool.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddressChainInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChainId = table.Column<int>(type: "INTEGER", nullable: false),
                    TxAmount = table.Column<int>(type: "INTEGER", nullable: false),
                    BaseBalance = table.Column<double>(type: "REAL", nullable: false),
                    FirstTxDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UsdcBalance = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressChainInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    CexAddress = table.Column<string>(type: "TEXT", nullable: true),
                    PrivateKey = table.Column<string>(type: "TEXT", nullable: false),
                    FantomInfoId = table.Column<int>(type: "INTEGER", nullable: false),
                    AvaxInfoId = table.Column<int>(type: "INTEGER", nullable: false),
                    PolygonInfoId = table.Column<int>(type: "INTEGER", nullable: false),
                    ArbitrumInfoId = table.Column<int>(type: "INTEGER", nullable: false),
                    OptimismInfoId = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalBalanceUsd = table.Column<double>(type: "REAL", nullable: false),
                    TotalTxAmount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountInfo_AddressChainInfos_ArbitrumInfoId",
                        column: x => x.ArbitrumInfoId,
                        principalTable: "AddressChainInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountInfo_AddressChainInfos_AvaxInfoId",
                        column: x => x.AvaxInfoId,
                        principalTable: "AddressChainInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountInfo_AddressChainInfos_FantomInfoId",
                        column: x => x.FantomInfoId,
                        principalTable: "AddressChainInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountInfo_AddressChainInfos_OptimismInfoId",
                        column: x => x.OptimismInfoId,
                        principalTable: "AddressChainInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountInfo_AddressChainInfos_PolygonInfoId",
                        column: x => x.PolygonInfoId,
                        principalTable: "AddressChainInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountInfo_ArbitrumInfoId",
                table: "AccountInfo",
                column: "ArbitrumInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountInfo_AvaxInfoId",
                table: "AccountInfo",
                column: "AvaxInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountInfo_FantomInfoId",
                table: "AccountInfo",
                column: "FantomInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountInfo_OptimismInfoId",
                table: "AccountInfo",
                column: "OptimismInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountInfo_PolygonInfoId",
                table: "AccountInfo",
                column: "PolygonInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountInfo");

            migrationBuilder.DropTable(
                name: "AddressChainInfos");
        }
    }
}
