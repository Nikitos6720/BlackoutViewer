using BlackoutViewer.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlackoutViewer.Configurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.HasKey(g => g.Id);
        
        builder.HasMany(g => g.Addresses)
               .WithOne(a => a.Group)
               .HasForeignKey(a => a.GroupId)
               .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasMany(g => g.Schedules)
               .WithOne(s => s.Group)
               .HasForeignKey(s => s.GroupId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}