using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aetherium.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewModelsForTimelinePosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Characters_CharacterModelId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_CharacterModelId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CharacterModelId",
                table: "Posts");

            migrationBuilder.CreateTable(
                name: "CommentModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentModel_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentModel_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReactionModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReactionModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReactionModel_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReactionModel_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CharacterId",
                table: "Posts",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentModel_CharacterId",
                table: "CommentModel",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentModel_PostId",
                table: "CommentModel",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionModel_CharacterId",
                table: "ReactionModel",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionModel_PostId",
                table: "ReactionModel",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Characters_CharacterId",
                table: "Posts",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Characters_CharacterId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "CommentModel");

            migrationBuilder.DropTable(
                name: "ReactionModel");

            migrationBuilder.DropIndex(
                name: "IX_Posts_CharacterId",
                table: "Posts");

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
    }
}
