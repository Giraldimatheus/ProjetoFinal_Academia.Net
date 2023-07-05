using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoFinal_Livros.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaCampoIdLivroEmLivroComentario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ComentarioPaiId",
                table: "LivroComentarios",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "IdLivro",
                table: "LivroComentarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdLivro",
                table: "LivroComentarios");

            migrationBuilder.AlterColumn<int>(
                name: "ComentarioPaiId",
                table: "LivroComentarios",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
