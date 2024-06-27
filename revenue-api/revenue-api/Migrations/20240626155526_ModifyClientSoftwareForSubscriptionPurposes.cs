using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace revenue_api.Migrations
{
    /// <inheritdoc />
    public partial class ModifyClientSoftwareForSubscriptionPurposes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_SubcriptionOffers_SubcriptionOfferSubscriptionOfferId",
                table: "Subscriptions");

            migrationBuilder.RenameColumn(
                name: "SubcriptionOfferSubscriptionOfferId",
                table: "Subscriptions",
                newName: "SubscriptionOfferId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_SubcriptionOfferSubscriptionOfferId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_SubscriptionOfferId");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ActiveUntil",
                table: "Subscriptions",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrentPeriodPaid",
                table: "Subscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<float>(
                name: "SoftwareVersion",
                table: "SubcriptionOffers",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_SubcriptionOffers_SubscriptionOfferId",
                table: "Subscriptions",
                column: "SubscriptionOfferId",
                principalTable: "SubcriptionOffers",
                principalColumn: "SubscriptionOfferId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_SubcriptionOffers_SubscriptionOfferId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "ActiveUntil",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "IsCurrentPeriodPaid",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "SoftwareVersion",
                table: "SubcriptionOffers");

            migrationBuilder.RenameColumn(
                name: "SubscriptionOfferId",
                table: "Subscriptions",
                newName: "SubcriptionOfferSubscriptionOfferId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_SubscriptionOfferId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_SubcriptionOfferSubscriptionOfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_SubcriptionOffers_SubcriptionOfferSubscriptionOfferId",
                table: "Subscriptions",
                column: "SubcriptionOfferSubscriptionOfferId",
                principalTable: "SubcriptionOffers",
                principalColumn: "SubscriptionOfferId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
