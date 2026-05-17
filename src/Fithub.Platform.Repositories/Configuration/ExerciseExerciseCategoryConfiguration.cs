using Fithub.Platform.Domain.Workout;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fithub.Platform.Repositories.Configuration;

public class ExerciseExerciseCategoryConfiguration : IEntityTypeConfiguration<ExerciseExerciseCategory>
{
    public void Configure(EntityTypeBuilder<ExerciseExerciseCategory> builder)
    {
        builder.ToTable("ExerciseExerciseCategories");

        builder.HasKey(e => new { e.ExerciseId, e.ExerciseCategoryId });

        builder.Property(e => e.ExerciseId)
            .HasColumnType("char(36)");

        builder.Property(e => e.ExerciseCategoryId)
            .HasColumnType("char(36)");

        builder.HasOne(e => e.Exercise)
            .WithMany(ex => ex.Categories)
            .HasForeignKey(e => e.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.ExerciseCategory)
            .WithMany(c => c.Exercises)
            .HasForeignKey(e => e.ExerciseCategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
