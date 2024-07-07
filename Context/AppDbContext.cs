using Microsoft.EntityFrameworkCore;
using TaskAPI.Models;

namespace TaskAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base( options ) { }

        public DbSet<Categoria>? Categorias { get; set; }
        public DbSet<Tarefa>? Tarefas { get; set; }
        public DbSet<Usuario>? Usuarios { get; set; }
    }
}
