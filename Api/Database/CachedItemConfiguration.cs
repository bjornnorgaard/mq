using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Database
{
    public class CachedItemConfiguration : IEntityTypeConfiguration<CachedItem>
    {
        public void Configure(EntityTypeBuilder<CachedItem> builder)
        {
            builder.HasKey(ci => ci.Id);

            builder.HasIndex(ci => ci.Key).IsUnique();
            builder.Property(ci => ci.Key).IsRequired();
            
            builder.Property(ci => ci.Value)
                .HasMaxLength(int.MaxValue)
                .IsRequired();
        }
    }
}