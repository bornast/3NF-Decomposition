using Microsoft.EntityFrameworkCore.Migrations;

namespace _3NF.Decomposition.Persistance.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Relations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Keys",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RelationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Keys_Relations_RelationId",
                        column: x => x.RelationId,
                        principalTable: "Relations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RelationId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_Relations_RelationId",
                        column: x => x.RelationId,
                        principalTable: "Relations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FminMembers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RelationId = table.Column<int>(nullable: false),
                    LeftSideMemberId = table.Column<int>(nullable: false),
                    RightSideMemberId = table.Column<int>(nullable: false),
                    Sequence = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FminMembers", x => new { x.Id, x.RelationId, x.LeftSideMemberId, x.RightSideMemberId, x.Sequence });
                    table.ForeignKey(
                        name: "FK_FminMembers_Members_LeftSideMemberId",
                        column: x => x.LeftSideMemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FminMembers_Relations_RelationId",
                        column: x => x.RelationId,
                        principalTable: "Relations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FminMembers_Members_RightSideMemberId",
                        column: x => x.RightSideMemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KeyMembers",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false),
                    MemberId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyMembers", x => new { x.KeyId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_KeyMembers_Keys_KeyId",
                        column: x => x.KeyId,
                        principalTable: "Keys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KeyMembers_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FminMembers_LeftSideMemberId",
                table: "FminMembers",
                column: "LeftSideMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_FminMembers_RelationId",
                table: "FminMembers",
                column: "RelationId");

            migrationBuilder.CreateIndex(
                name: "IX_FminMembers_RightSideMemberId",
                table: "FminMembers",
                column: "RightSideMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_KeyMembers_MemberId",
                table: "KeyMembers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Keys_RelationId",
                table: "Keys",
                column: "RelationId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_RelationId",
                table: "Members",
                column: "RelationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FminMembers");

            migrationBuilder.DropTable(
                name: "KeyMembers");

            migrationBuilder.DropTable(
                name: "Keys");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Relations");
        }
    }
}
