using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flowtap_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultStoreIdToAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DefaultStoreId",
                table: "AppUsers",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultStoreId",
                table: "AppUsers");
        }
    }
}
