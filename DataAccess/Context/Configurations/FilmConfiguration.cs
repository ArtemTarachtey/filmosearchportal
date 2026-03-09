using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Context.Configurations
{
    public class FilmConfiguration : IEntityTypeConfiguration<Film>
    {
        public void Configure(EntityTypeBuilder<Film> builder)
        {
            builder.HasKey(f => f.Id);
            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(f => f.Description)
                .HasMaxLength(10000)
                .IsRequired();

            builder.Property(f => f.Duration)
                .IsRequired();

            builder.ToTable((t) =>
            {
                t.HasCheckConstraint("CK_Film_Duration", "Duration > 0 AND Duration <= 2000");
            });
        }
    }
}
