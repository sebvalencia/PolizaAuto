using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PolizaAuto.Data
{
    public class InsuranceContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public InsuranceContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = _configuration.GetConnectionString("InsuranceDatabase");
            optionsBuilder.UseMongoDb(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuraciones adicionales del modelo si es necesario
        }
    }
}