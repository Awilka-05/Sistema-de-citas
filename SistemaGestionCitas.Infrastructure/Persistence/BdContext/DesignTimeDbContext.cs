using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SistemaGestionCitas.Infrastructure.Persistence.BdContext;

public class DesignTimeDbContext : IDesignTimeDbContextFactory<SistemaCitasDbContext>
{
    public SistemaCitasDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SistemaCitasDbContext>();
        optionsBuilder.UseSqlServer("Server=MSI\\ALNA_DESARRODatabase=SistemaCitas;Trusted_Connection=True;TrustServerCertificate=True;");

        return new SistemaCitasDbContext(optionsBuilder.Options);
    }    
}