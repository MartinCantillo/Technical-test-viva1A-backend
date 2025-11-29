
using Models;
using Microsoft.EntityFrameworkCore;


namespace ApiPacientes.Data
{
public class ApplicationDbContext : DbContext
{
public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
: base(options) { }


public DbSet<Patient> Patients { get; set; } = null!;


protected override void OnModelCreating(ModelBuilder modelBuilder)
{
base.OnModelCreating(modelBuilder);


modelBuilder.Entity<Patient>(entity => {
entity.HasKey(e => e.PatientId);
entity.HasIndex(e => new { e.DocumentType, e.DocumentNumber }).IsUnique();
entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
});
}
}
}