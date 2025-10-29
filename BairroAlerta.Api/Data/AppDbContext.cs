using Microsoft.EntityFrameworkCore;
using BairroAlerta.Api.Models;

namespace BairroAlerta.Api.Data
{
    public class AlertaContext : DbContext
    {
        public AlertaContext(DbContextOptions<AlertaContext> options)
            : base(options) { }

        public DbSet<Alerta> Alertas { get; set; }
    }
}
