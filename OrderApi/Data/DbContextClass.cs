using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrderAPI.Models;

namespace OrderAPI.Data
{
    public class DbContextClass : DbContext
    {
        IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
        private readonly string _connectionString;

        public DbContextClass(DbContextOptions<DbContextClass> options, bool forTest = false) : base(options)
        {
            if (!forTest)
            {
                configurationBuilder.AddJsonFile("AppSettings.json");
                IConfiguration configuration = configurationBuilder.Build();
                _connectionString = configuration.GetConnectionString("DefaultConnection");
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!(string.IsNullOrEmpty(_connectionString)))
                options.UseSqlServer(_connectionString);
                //options.UseNpgsql(_connectionString);
        }

        public virtual DbSet<Order> Orders { get; set; }
    }
}
