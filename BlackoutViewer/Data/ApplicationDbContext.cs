using BlackoutViewer.Configurations;
using BlackoutViewer.Models;

using Microsoft.EntityFrameworkCore;

namespace BlackoutViewer.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Schedule> schedules { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AddressConfiguration());
        modelBuilder.ApplyConfiguration(new GroupConfiguration());
        modelBuilder.ApplyConfiguration(new ScheduleConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}