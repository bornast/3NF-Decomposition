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
                name: "Attributes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RelationId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attributes_Relations_RelationId",
                        column: x => x.RelationId,
                        principalTable: "Relations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "FminAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RelationId = table.Column<int>(nullable: false),
                    LeftSideAttributeId = table.Column<int>(nullable: false),
                    RightSideAttributeId = table.Column<int>(nullable: false),
                    Sequence = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FminAttributes", x => new { x.Id, x.RelationId, x.LeftSideAttributeId, x.RightSideAttributeId, x.Sequence });
                    table.ForeignKey(
                        name: "FK_FminAttributes_Attributes_LeftSideAttributeId",
                        column: x => x.LeftSideAttributeId,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FminAttributes_Relations_RelationId",
                        column: x => x.RelationId,
                        principalTable: "Relations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FminAttributes_Attributes_RightSideAttributeId",
                        column: x => x.RightSideAttributeId,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KeyAttributes",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false),
                    AttributeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyAttributes", x => new { x.KeyId, x.AttributeId });
                    table.ForeignKey(
                        name: "FK_KeyAttributes_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KeyAttributes_Keys_KeyId",
                        column: x => x.KeyId,
                        principalTable: "Keys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_RelationId",
                table: "Attributes",
                column: "RelationId");

            migrationBuilder.CreateIndex(
                name: "IX_FminAttributes_LeftSideAttributeId",
                table: "FminAttributes",
                column: "LeftSideAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_FminAttributes_RelationId",
                table: "FminAttributes",
                column: "RelationId");

            migrationBuilder.CreateIndex(
                name: "IX_FminAttributes_RightSideAttributeId",
                table: "FminAttributes",
                column: "RightSideAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_KeyAttributes_AttributeId",
                table: "KeyAttributes",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_Keys_RelationId",
                table: "Keys",
                column: "RelationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FminAttributes");

            migrationBuilder.DropTable(
                name: "KeyAttributes");

            migrationBuilder.DropTable(
                name: "Attributes");

            migrationBuilder.DropTable(
                name: "Keys");

            migrationBuilder.DropTable(
                name: "Relations");
        }
    }
}
