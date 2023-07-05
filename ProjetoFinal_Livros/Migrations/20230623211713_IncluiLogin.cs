using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoFinal_Livros.Migrations
{
    /// <inheritdoc />
    public partial class IncluiLogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Login",
                table: "Usuarios");
        }
    }
}
