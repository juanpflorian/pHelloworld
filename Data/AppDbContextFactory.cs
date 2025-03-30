using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace pHelloworld.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            
            var basePath = Directory.GetCurrentDirectory();
            Console.WriteLine($"Base Path: {basePath}");

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath) 
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            Console.WriteLine($"Cadena de conexión: {connectionString}");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("No se encontró la cadena de conexión. Verifica appsettings.json.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
