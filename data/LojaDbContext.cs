using Microsoft.EntityFrameworkCore;

namespace loja.data
{
    public class LojaDbContext : DbContext {
        public LojaDbContext(DbContextOptions<LojaDbContext> options) : base(options){}
        public DbSet<ProducesResponseTypeMetadata> Produtos {get; set;}

    }
}