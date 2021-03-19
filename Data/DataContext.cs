using Microsoft.EntityFrameworkCore;
using Shop.Modelos;

namespace Shop.Data
{
    public class DataContext : DbContext
    {
       public DataContext(DbContextOptions<DataContext> options)
         :base(options)
         { }

         public DbSet<Produto> Produtos { get; set; }

         public DbSet<Categorias> Categorias { get; set; }

         public DbSet<Usuario> Usuarios { get; set; } 
    }
}