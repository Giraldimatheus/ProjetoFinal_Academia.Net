using Microsoft.EntityFrameworkCore;
using ProjetoFinal_Livros.Models;

namespace ProjetoFinal_Livros
{
    public class Context: DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<LivroComentario> LivroComentarios { get; set; }
        public DbSet<LivrosFavoritos> LivrosFavoritos { get;set; }
        public Context()
        {

        }

        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Data Source=MAT\\MSSQLSERVER1;Initial Catalog=ProjetoFinal;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LivrosFavoritos>().HasOne(e=>e.usuario).WithMany(e=>e.LivrosFavoritos).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<LivroComentario>().HasOne(e => e.Usuario).WithMany(e => e.LivrosComentario).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
