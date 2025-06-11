using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aetherium.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewDbSets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentModel_Characters_CharacterId",
                table: "CommentModel");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentModel_Posts_PostId",
                table: "CommentModel");

            migrationBuilder.DropForeignKey(
                name: "FK_ReactionModel_Characters_CharacterId",
                table: "ReactionModel");

            migrationBuilder.DropForeignKey(
                name: "FK_ReactionModel_Posts_PostId",
                table: "ReactionModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReactionModel",
                table: "ReactionModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentModel",
                table: "CommentModel");

            migrationBuilder.RenameTable(
                name: "ReactionModel",
                newName: "Reactions");

            migrationBuilder.RenameTable(
                name: "CommentModel",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_ReactionModel_PostId",
                table: "Reactions",
                newName: "IX_Reactions_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_ReactionModel_CharacterId",
                table: "Reactions",
                newName: "IX_Reactions_CharacterId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentModel_PostId",
                table: "Comments",
                newName: "IX_Comments_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentModel_CharacterId",
                table: "Comments",
                newName: "IX_Comments_CharacterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reactions",
                table: "Reactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Characters_CharacterId",
                table: "Comments",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_Characters_CharacterId",
                table: "Reactions",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_Posts_PostId",
                table: "Reactions",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Characters_CharacterId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Characters_CharacterId",
                table: "Reactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Posts_PostId",
                table: "Reactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reactions",
                table: "Reactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Reactions",
                newName: "ReactionModel");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "CommentModel");

            migrationBuilder.RenameIndex(
                name: "IX_Reactions_PostId",
                table: "ReactionModel",
                newName: "IX_ReactionModel_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Reactions_CharacterId",
                table: "ReactionModel",
                newName: "IX_ReactionModel_CharacterId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_PostId",
                table: "CommentModel",
                newName: "IX_CommentModel_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CharacterId",
                table: "CommentModel",
                newName: "IX_CommentModel_CharacterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReactionModel",
                table: "ReactionModel",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentModel",
                table: "CommentModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentModel_Characters_CharacterId",
                table: "CommentModel",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentModel_Posts_PostId",
                table: "CommentModel",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReactionModel_Characters_CharacterId",
                table: "ReactionModel",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReactionModel_Posts_PostId",
                table: "ReactionModel",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
