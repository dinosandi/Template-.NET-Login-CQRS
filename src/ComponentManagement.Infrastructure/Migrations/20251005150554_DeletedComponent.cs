using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComponentManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeletedComponent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComponentActivities_Components_ComponentId",
                table: "ComponentActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_PartComponentAPLs_Components_PartComponentId",
                table: "PartComponentAPLs");

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentActivities_Components_ComponentId",
                table: "ComponentActivities",
                column: "ComponentId",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartComponentAPLs_Components_PartComponentId",
                table: "PartComponentAPLs",
                column: "PartComponentId",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComponentActivities_Components_ComponentId",
                table: "ComponentActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_PartComponentAPLs_Components_PartComponentId",
                table: "PartComponentAPLs");

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentActivities_Components_ComponentId",
                table: "ComponentActivities",
                column: "ComponentId",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PartComponentAPLs_Components_PartComponentId",
                table: "PartComponentAPLs",
                column: "PartComponentId",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
