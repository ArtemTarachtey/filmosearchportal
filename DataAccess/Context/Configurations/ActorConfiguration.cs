using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Context.Configurations
{
    public class ActorConfiguration: IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.LastName)
                .HasMaxLength(100);

            builder.Property(a => a.Age)
                .IsRequired();

            builder.Property(a => a.Rating)
                .IsRequired();

            builder.HasMany(a => a.Films)
                .WithMany(f => f.Actors)
                .UsingEntity(t => t.ToTable("ActorFilm")); // явная junction таблица

            // ограничение на данные на уровне конфига
            builder.ToTable((t) =>
            {
                t.HasCheckConstraint("CK_Actor_Age", "Age >= 0 AND Age <= 200");
                t.HasCheckConstraint("CK_Actor_Rating", "Rating >= 0 AND Rating <= 10");
            });
        }
    }
}
