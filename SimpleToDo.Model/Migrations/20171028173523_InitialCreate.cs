using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleToDo.Model.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "List",
                columns: table => new
                {
                    ListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_List", x => x.ListId);
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Done = table.Column<bool>(type: "bit", nullable: false),
                    ListId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.TaskId);
                    table.ForeignKey(
                        name: "FK_Task_List_ListId",
                        column: x => x.ListId,
                        principalTable: "List",
                        principalColumn: "ListId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Task_ListId",
                table: "Task",
                column: "ListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "List");
        }
    }
}
