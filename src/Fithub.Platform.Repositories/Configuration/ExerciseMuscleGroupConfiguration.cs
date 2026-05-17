using Fithub.Platform.Domain.Workout;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fithub.Platform.Repositories.Configuration;

public class ExerciseMuscleGroupConfiguration : IEntityTypeConfiguration<ExerciseMuscleGroup>
{
    public void Configure(EntityTypeBuilder<ExerciseMuscleGroup> builder)
    {
        builder.ToTable("ExerciseMuscleGroups");

        builder.HasKey(e => new { e.ExerciseId, e.MuscleGroup });

        builder.Property(e => e.ExerciseId)
            .HasColumnType("char(36)");

        builder.Property(e => e.MuscleGroup)
            .HasConversion<int>();

        builder.Property(e => e.IsPrimary)
            .IsRequired()
            .HasDefaultValue(true);
    }
}
