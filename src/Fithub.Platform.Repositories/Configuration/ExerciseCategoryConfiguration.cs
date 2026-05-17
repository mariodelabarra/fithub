using Fithub.Platform.Domain.Workout;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fithub.Platform.Repositories.Configuration;

public class ExerciseCategoryConfiguration : IEntityTypeConfiguration<ExerciseCategory>
{
    public void Configure(EntityTypeBuilder<ExerciseCategory> builder)
    {
        builder.ToTable("ExerciseCategories");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnType("char(36)")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(e => e.Name).IsUnique();

        builder.Property(e => e.Description)
            .HasMaxLength(500);

        builder.Property(e => e.CreatedOn).IsRequired();
        builder.Property(e => e.ModifiedOn);

        builder.HasData(
            new ExerciseCategory { Id = Guid.Parse("11111111-0000-0000-0000-000000000001"), CreatedOn = DateTime.UtcNow, Name = "Compound" },
            new ExerciseCategory { Id = Guid.Parse("11111111-0000-0000-0000-000000000002"), CreatedOn = DateTime.UtcNow, Name = "Isolation" },
            new ExerciseCategory { Id = Guid.Parse("11111111-0000-0000-0000-000000000003"), CreatedOn = DateTime.UtcNow, Name = "HIIT" },
            new ExerciseCategory { Id = Guid.Parse("11111111-0000-0000-0000-000000000004"), CreatedOn = DateTime.UtcNow, Name = "Rehabilitation" },
            new ExerciseCategory { Id = Guid.Parse("11111111-0000-0000-0000-000000000005"), CreatedOn = DateTime.UtcNow, Name = "Mobility" },
            new ExerciseCategory { Id = Guid.Parse("11111111-0000-0000-0000-000000000006"), CreatedOn = DateTime.UtcNow, Name = "Powerlifting" },
            new ExerciseCategory { Id = Guid.Parse("11111111-0000-0000-0000-000000000007"), CreatedOn = DateTime.UtcNow, Name = "Bodybuilding" },
            new ExerciseCategory { Id = Guid.Parse("11111111-0000-0000-0000-000000000008"), CreatedOn = DateTime.UtcNow, Name = "Functional" }
        );
    }
}
