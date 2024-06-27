using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace revenue_api.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionsUpdatePaymentToIncludeSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ContractId",
                table: "Payments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubcriptionOffers",
                columns: table => new
                {
                    SubscriptionOfferId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    RenewalPeriod = table.Column<int>(type: "int", nullable: false),
                    SoftwareId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    SubscriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubcriptionOfferSubscriptionOfferId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.SubscriptionId);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscriptions_SubcriptionOffers_SubcriptionOfferSubscriptionOfferId",
                        column: x => x.SubcriptionOfferSubscriptionOfferId,
                        principalTable: "SubcriptionOffers",
                        principalColumn: "SubscriptionOfferId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_SubscriptionId",
                table: "Payments",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubcriptionOffers_SoftwareId",
                table: "SubcriptionOffers",
                column: "SoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_ClientId",
                table: "Subscriptions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SubcriptionOfferSubscriptionOfferId",
                table: "Subscriptions",
                column: "SubcriptionOfferSubscriptionOfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Subscriptions_SubscriptionId",
                table: "Payments",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "SubscriptionId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Subscriptions_SubscriptionId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "SubcriptionOffers");

            migrationBuilder.DropIndex(
                name: "IX_Payments_SubscriptionId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "ContractId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
