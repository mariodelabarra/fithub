using Fithub.Platform.Domain.Workout;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fithub.Platform.Repositories.Configuration;

public class WorkoutSessionConfiguration : IEntityTypeConfiguration<WorkoutSession>
{
    public void Configure(EntityTypeBuilder<WorkoutSession> builder)
    {
        builder.ToTable("WorkoutSessions");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnType("char(36)")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.UserId)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(e => e.WorkoutDayId)
            .HasColumnType("char(36)");

        builder.Property(e => e.Notes)
            .HasMaxLength(2000);

        builder.Property(e => e.StartedAt).IsRequired();

        builder.Property(e => e.CreatedOn).IsRequired();
        builder.Property(e => e.ModifiedOn);

        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => new { e.UserId, e.StartedAt });

        builder.HasOne(e => e.WorkoutDay)
            .WithMany(d => d.Sessions)
            .HasForeignKey(e => e.WorkoutDayId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(e => e.ExerciseLogs)
            .WithOne(l => l.WorkoutSession)
            .HasForeignKey(l => l.WorkoutSessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
