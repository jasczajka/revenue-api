using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace revenue_api.Migrations
{
    /// <inheritdoc />
    public partial class FixSubOfferNamingTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_SubcriptionOffers_SubscriptionOfferId",
                table: "Subscriptions");

            migrationBuilder.DropTable(
                name: "SubcriptionOffers");

            migrationBuilder.CreateTable(
                name: "SubscriptionOffers",
                columns: table => new
                {
                    SubscriptionOfferId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    RenewalPeriod = table.Column<int>(type: "int", nullable: false),
                    SoftwareVersion = table.Column<float>(type: "real", nullable: false),
                    SoftwareId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionOffers", x => x.SubscriptionOfferId);
                    table.ForeignKey(
                        name: "FK_SubscriptionOffers_Softwares_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "Softwares",
                        principalColumn: "SoftwareId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionOffers_SoftwareId",
                table: "SubscriptionOffers",
                column: "SoftwareId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_SubscriptionOffers_SubscriptionOfferId",
                table: "Subscriptions",
                column: "SubscriptionOfferId",
                principalTable: "SubscriptionOffers",
                principalColumn: "SubscriptionOfferId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_SubscriptionOffers_SubscriptionOfferId",
                table: "Subscriptions");

            migrationBuilder.DropTable(
                name: "SubscriptionOffers");

            migrationBuilder.CreateTable(
                name: "SubcriptionOffers",
                columns: table => new
                {
                    SubscriptionOfferId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoftwareId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    RenewalPeriod = table.Column<int>(type: "int", nullable: false),
                    SoftwareVersion = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubcriptionOffers", x => x.SubscriptionOfferId);
                    table.ForeignKey(
                        name: "FK_SubcriptionOffers_Softwares_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "Softwares",
                        principalColumn: "SoftwareId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubcriptionOffers_SoftwareId",
                table: "SubcriptionOffers",
                column: "SoftwareId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_SubcriptionOffers_SubscriptionOfferId",
                table: "Subscriptions",
                column: "SubscriptionOfferId",
                principalTable: "SubcriptionOffers",
                principalColumn: "SubscriptionOfferId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
