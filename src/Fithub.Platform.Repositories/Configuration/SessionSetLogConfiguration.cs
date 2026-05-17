using Fithub.Platform.Domain.Workout;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fithub.Platform.Repositories.Configuration;

public class SessionSetLogConfiguration : IEntityTypeConfiguration<SessionSetLog>
{
    public void Configure(EntityTypeBuilder<SessionSetLog> builder)
    {
        builder.ToTable("SessionSetLogs");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnType("char(36)")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.SessionExerciseLogId)
            .HasColumnType("char(36)")
            .IsRequired();

        builder.Property(e => e.SetType)
            .HasConversion<int>();

        builder.Property(e => e.ActualWeightKg)
            .HasColumnType("decimal(6,2)");

        builder.Property(e => e.ActualDistanceMeters)
            .HasColumnType("decimal(8,2)");

        builder.Property(e => e.IsCompleted)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedOn).IsRequired();
        builder.Property(e => e.ModifiedOn);

        builder.HasIndex(e => e.SessionExerciseLogId);
    }
}
