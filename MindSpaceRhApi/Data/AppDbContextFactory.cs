using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Oracle.EntityFrameworkCore; // Importante para usar UseOracle

namespace MindSpaceRhApi.Data
{
    // Esta classe permite que o EF Core crie o AppDbContext para Migrations
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // --- INSERÇÃO MANUAL DA STRING DE CONEXÃO ORACLE ---
            // Como o comando Add-Migration ignora o appsettings.json, inserimos a string aqui.
            string connectionString = "User Id=rm561120;Password=130305;Data Source=oracle.fiap.com.br:1521/ORCL";

            // Diz ao EF Core para usar o driver Oracle com a string de conexão
            optionsBuilder.UseOracle(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}