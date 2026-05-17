using Fithub.Platform.Domain.Workout;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fithub.Platform.Repositories.Configuration;

public class SessionExerciseLogConfiguration : IEntityTypeConfiguration<SessionExerciseLog>
{
    public void Configure(EntityTypeBuilder<SessionExerciseLog> builder)
    {
        builder.ToTable("SessionExerciseLogs");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnType("char(36)")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.WorkoutSessionId)
            .HasColumnType("char(36)")
            .IsRequired();

        builder.Property(e => e.ExerciseId)
            .HasColumnType("char(36)")
            .IsRequired();

        builder.Property(e => e.WorkoutExerciseId)
            .HasColumnType("char(36)");

        builder.Property(e => e.Notes)
            .HasMaxLength(1000);

        builder.Property(e => e.CreatedOn).IsRequired();
        builder.Property(e => e.ModifiedOn);

        builder.HasIndex(e => e.WorkoutSessionId);

        builder.HasOne(e => e.Exercise)
            .WithMany()
            .HasForeignKey(e => e.ExerciseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.WorkoutExercise)
            .WithMany()
            .HasForeignKey(e => e.WorkoutExerciseId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(e => e.SetLogs)
            .WithOne(s => s.SessionExerciseLog)
            .HasForeignKey(s => s.SessionExerciseLogId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
