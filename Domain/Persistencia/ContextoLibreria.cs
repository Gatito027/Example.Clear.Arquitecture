using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Persistencia
{
    public class ContextoLibreria : DbContext
    {
        public ContextoLibreria(DbContextOptions<ContextoLibreria> options) : base(options)
        {

        }
        public DbSet<LibreriaMaterial> LibreriaMaterial { get; set; }
    }
}
