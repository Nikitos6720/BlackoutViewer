using BlackoutViewer.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlackoutViewer.Configurations;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.ToTable("Schedules");
        builder.HasKey(s => s.Id);

        builder.HasOne(s => s.Group)
               .WithMany(g => g.Schedules)
               .HasForeignKey(s => s.GroupId);
    }
}