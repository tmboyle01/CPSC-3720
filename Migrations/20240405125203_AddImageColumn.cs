using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TigerTix.Migrations
{
    /// <inheritdoc />
    public partial class AddImageColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "price",
                table: "Tickets",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "float");

            migrationBuilder.AddColumn<byte[]>(
                name: "EventImage",
                table: "Events",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventImage",
                table: "Events");

            //migrationBuilder.AlterColumn<int>(
            //    name: "price",
            //    table: "Tickets",
            //    type: "float",
            //    nullable: false,
            //    oldClrType: typeof(float),
            //    oldType: "float");
        }
    }
}
