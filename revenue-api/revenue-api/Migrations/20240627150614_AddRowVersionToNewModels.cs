using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace revenue_api.Migrations
{
    /// <inheritdoc />
    public partial class AddRowVersionToNewModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Subscriptions",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "SubscriptionOffers",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "SubscriptionOffers");
        }
    }
}
