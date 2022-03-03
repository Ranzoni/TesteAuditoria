using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TesteAuditoria
{
    public class TesteAuditoriaContext : DbContext
    {
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
        public DbSet<Auditoria> Auditorias { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TesteAuditoria;Trusted_Connection=true;");
        }

        public override int SaveChanges()
        {
            var entriesChanged = ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Unchanged && e.State != EntityState.Detached).ToList();

            foreach (var entryChanged in entriesChanged)
            {
                var auditoria = new Auditoria();
                if (entryChanged.State != EntityState.Modified)
                {
                    var acao = entryChanged.State == EntityState.Added ? "Inclusão" : "Remoção";
                    var campos = "";
                    foreach (var propriedade in entryChanged.Properties)
                        campos += $"{propriedade.Metadata.Name} = '{propriedade.CurrentValue}' |";

                    auditoria.Acao = acao;
                    auditoria.Descricao = campos;
                }
                else
                {
                    var campos = "";
                    foreach (var propriedade in entryChanged.Properties)
                        campos += $"{propriedade.Metadata.Name}: Valor antigo = '{propriedade.OriginalValue}'; Valor novo = '{propriedade.CurrentValue}' |";

                    auditoria.Acao = "Modificação";
                    auditoria.Descricao = campos;
                }
                Auditorias.Add(auditoria);
            }

            return base.SaveChanges();
        }
    }
}
