using Fithub.Platform.Domain.Workout;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fithub.Platform.Repositories.Configuration;

public class WorkoutExerciseSetConfiguration : IEntityTypeConfiguration<WorkoutExerciseSet>
{
    public void Configure(EntityTypeBuilder<WorkoutExerciseSet> builder)
    {
        builder.ToTable("WorkoutExerciseSets");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnType("char(36)")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.WorkoutExerciseId)
            .HasColumnType("char(36)")
            .IsRequired();

        builder.Property(e => e.SetType)
            .HasConversion<int>();

        builder.Property(e => e.TargetWeightKg)
            .HasColumnType("decimal(6,2)");

        builder.Property(e => e.TargetDistanceMeters)
            .HasColumnType("decimal(8,2)");

        builder.Property(e => e.CreatedOn).IsRequired();
        builder.Property(e => e.ModifiedOn);

        builder.HasIndex(e => e.WorkoutExerciseId);
    }
}
