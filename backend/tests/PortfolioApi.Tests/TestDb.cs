using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;

namespace PortfolioApi.Tests;

/// <summary>
/// Cria um PortfolioContext isolado usando SQLite in-memory por teste.
/// </summary>
public static class TestDb
{
    public static (PortfolioContext context, SqliteConnection connection) CreateContext()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<PortfolioContext>()
            .UseSqlite(connection)
            .Options;

        var context = new PortfolioContext(options);
        context.Database.EnsureCreated();
        return (context, connection);
    }
}
