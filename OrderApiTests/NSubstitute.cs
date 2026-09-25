using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using ProductApi.Models;
using ProductAPI.Data;
using ProductAPI.Models;
using ProductAPI.Services;
using Shouldly;

public class TestDbContextClass : DbContextClass
{
    private readonly SqliteConnection _connection;
    public TestDbContextClass(DbContextOptions<DbContextClass> options, SqliteConnection connection) : base(options) => _connection = connection;
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite(_connection);
}

public class ServiceTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<DbContextClass> _options;
    private readonly ILogger<ProductService> _productLogger = Substitute.For<ILogger<ProductService>>();

    public ServiceTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // Enforce relational rules in SQLite
        using (var command = _connection.CreateCommand())
        {
            command.CommandText = "PRAGMA foreign_keys = ON;";
            command.ExecuteNonQuery();
        }

        _options = new DbContextOptionsBuilder<DbContextClass>().Options;
        using var context = new TestDbContextClass(_options, _connection);
        context.Database.EnsureCreated();
    }

    [Fact]
    public void DeleteCategory_WithLinkedProduct_BehaviorsDiverge()
    {
        // 1. Arrange: Create a Category and link a Product to it
        var categoryId = 1;
        using (var context = new TestDbContextClass(_options, _connection))
        {
            var category = new ProductCategory { ProductCategoryId = categoryId, ProductCategoryName = "Electronics" };
            context.Categories.Add(category);

            context.Products.Add(new Product
            {
                ProductId = 50,
                ProductName = "Phone",
                ProductDescription = "Smartphone",
                ProductCategoryId = categoryId // Relational link
            });
            context.SaveChanges();
        }

        bool result = false;

        // 2. Act: Try to delete the Category while the Product still depends on it
        using (var context = new TestDbContextClass(_options, _connection))
        {
            try
            {
                var categoryToDelete = context.Categories.Find(categoryId);
                context.Categories.Remove(categoryToDelete!);
                context.SaveChanges();

                result = true; // EF In-Memory reaches here successfully!
            }
            catch (DbUpdateException)
            {
                result = false; // SQLite triggers this catch block because of the foreign key constraint
            }
        }

        // 3. Assert: The exact same line of code
        // -> EF Core In-Memory: 'result' is true. This line PASSES.
        // -> SQLite In-Memory:  'result' is false. This line FAILS.
        result.ShouldBe(true);
    }

    public void Dispose()
    {
        _connection?.Close();
        _connection?.Dispose();
    }
}
