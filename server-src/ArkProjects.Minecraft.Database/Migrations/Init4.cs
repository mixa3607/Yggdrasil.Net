using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ArkProjects.Minecraft.Database.Migrations
{
    /// <inheritdoc />
    public partial class Init4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Users",
                schema: "public",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "UserProfiles",
                schema: "public",
                newName: "UserProfiles");

            migrationBuilder.RenameTable(
                name: "UserAccessTokens",
                schema: "public",
                newName: "UserAccessTokens");

            migrationBuilder.RenameTable(
                name: "TempCodes",
                schema: "public",
                newName: "TempCodes");

            migrationBuilder.RenameTable(
                name: "Servers",
                schema: "public",
                newName: "Servers");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                schema: "public",
                newName: "RefreshTokens");

            migrationBuilder.CreateTable(
                name: "UserServerJoins",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ExpiredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ServerInstanceId = table.Column<string>(type: "text", nullable: false),
                    UserProfileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserServerJoins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserServerJoins_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserServerJoins_UserProfileId",
                table: "UserServerJoins",
                column: "UserProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserServerJoins");

            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Users",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "UserProfiles",
                newName: "UserProfiles",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "UserAccessTokens",
                newName: "UserAccessTokens",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "TempCodes",
                newName: "TempCodes",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Servers",
                newName: "Servers",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                newName: "RefreshTokens",
                newSchema: "public");
        }
    }
}
