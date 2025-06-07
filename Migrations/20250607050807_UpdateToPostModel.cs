using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aetherium.Migrations
{
    /// <inheritdoc />
    public partial class UpdateToPostModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Characters_CharacterId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_CharacterId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "IsEdited",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "VisibleBy",
                table: "Posts",
                newName: "PostContent");

            migrationBuilder.RenameColumn(
                name: "EditedBy",
                table: "Posts",
                newName: "PrivacyLevel");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Posts",
                newName: "CreatedOn");

            migrationBuilder.AddColumn<int>(
                name: "AllowedRelationshipType",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CharacterModelId",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CharacterModelId",
                table: "Posts",
                column: "CharacterModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Characters_CharacterModelId",
                table: "Posts",
                column: "CharacterModelId",
                principalTable: "Characters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Characters_CharacterModelId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_CharacterModelId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "AllowedRelationshipType",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CharacterModelId",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "PrivacyLevel",
                table: "Posts",
                newName: "EditedBy");

            migrationBuilder.RenameColumn(
                name: "PostContent",
                table: "Posts",
                newName: "VisibleBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Posts",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsEdited",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CharacterId",
                table: "Posts",
                column: "CharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Characters_CharacterId",
                table: "Posts",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
