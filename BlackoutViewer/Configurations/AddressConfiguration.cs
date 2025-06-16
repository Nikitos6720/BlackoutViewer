using BlackoutViewer.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlackoutViewer.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses");
        builder.HasKey(a => a.Id);

        builder.HasOne(a => a.Group)
               .WithMany(g => g.Addresses)
               .HasForeignKey(a => a.GroupId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
