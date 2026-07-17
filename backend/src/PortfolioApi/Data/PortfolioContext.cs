using Microsoft.EntityFrameworkCore;
using PortfolioApi.Models;

namespace PortfolioApi.Data;

public class PortfolioContext : DbContext
{
    public PortfolioContext(DbContextOptions<PortfolioContext> options) : base(options)
    {
    }

    public DbSet<Ativo> Ativos => Set<Ativo>();
    public DbSet<Ordem> Ordens => Set<Ordem>();
    public DbSet<WatchlistItem> Watchlist => Set<WatchlistItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ativo>(e =>
        {
            e.Property(a => a.Ticker).IsRequired().HasMaxLength(10);
            e.Property(a => a.PrecoMedio).HasColumnType("decimal(18,2)");
            e.HasMany(a => a.Ordens)
             .WithOne(o => o.Ativo)
             .HasForeignKey(o => o.AtivoId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Ordem>(e =>
        {
            e.Property(o => o.Preco).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<WatchlistItem>(e =>
        {
            e.Property(w => w.Ticker).IsRequired().HasMaxLength(10);
            e.Property(w => w.PrecoAlvo).HasColumnType("decimal(18,2)");
        });
    }
}
