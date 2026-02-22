using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComponentManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fieldpriorty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_APLs_Components_PartComponentId",
                table: "APLs");

            migrationBuilder.DropForeignKey(
                name: "FK_ComponentActivities_Components_ComponentId",
                table: "ComponentActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_PartComponentAPLs_APLs_APLId",
                table: "PartComponentAPLs");

            migrationBuilder.DropForeignKey(
                name: "FK_PartComponentAPLs_Components_PartComponentId",
                table: "PartComponentAPLs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartComponentAPLs",
                table: "PartComponentAPLs");

            migrationBuilder.DropIndex(
                name: "IX_PartComponentAPLs_PartComponentId",
                table: "PartComponentAPLs");

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "APLParts",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartComponentAPLs",
                table: "PartComponentAPLs",
                columns: new[] { "PartComponentId", "APLId" });

            migrationBuilder.AddForeignKey(
                name: "FK_APLs_Components_PartComponentId",
                table: "APLs",
                column: "PartComponentId",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentActivities_Components_ComponentId",
                table: "ComponentActivities",
                column: "ComponentId",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PartComponentAPLs_APLs_APLId",
                table: "PartComponentAPLs",
                column: "APLId",
                principalTable: "APLs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PartComponentAPLs_Components_PartComponentId",
                table: "PartComponentAPLs",
                column: "PartComponentId",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_APLs_Components_PartComponentId",
                table: "APLs");

            migrationBuilder.DropForeignKey(
                name: "FK_ComponentActivities_Components_ComponentId",
                table: "ComponentActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_PartComponentAPLs_APLs_APLId",
                table: "PartComponentAPLs");

            migrationBuilder.DropForeignKey(
                name: "FK_PartComponentAPLs_Components_PartComponentId",
                table: "PartComponentAPLs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartComponentAPLs",
                table: "PartComponentAPLs");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "APLParts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartComponentAPLs",
                table: "PartComponentAPLs",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PartComponentAPLs_PartComponentId",
                table: "PartComponentAPLs",
                column: "PartComponentId");

            migrationBuilder.AddForeignKey(
                name: "FK_APLs_Components_PartComponentId",
                table: "APLs",
                column: "PartComponentId",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentActivities_Components_ComponentId",
                table: "ComponentActivities",
                column: "ComponentId",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartComponentAPLs_APLs_APLId",
                table: "PartComponentAPLs",
                column: "APLId",
                principalTable: "APLs",
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
    }
}
