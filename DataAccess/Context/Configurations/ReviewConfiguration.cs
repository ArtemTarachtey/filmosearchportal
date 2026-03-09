using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Context.Configurations
{
    public class ReviewConfiguration: IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.Text)
                .IsRequired()
                .HasMaxLength(3000);

            builder.Property(r => r.Stars)
                .IsRequired();

            builder.HasOne(r => r.Film)
                .WithMany(f => f.Reviews)
                .HasForeignKey(r => r.FilmId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable((t) =>
            {
                t.HasCheckConstraint("CK_Review_Stars", "Stars >= 0 AND Stars <= 5");
            });

        }
    }
}
