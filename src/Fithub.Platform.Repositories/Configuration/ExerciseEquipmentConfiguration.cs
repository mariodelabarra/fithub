using Fithub.Platform.Domain.Workout;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fithub.Platform.Repositories.Configuration;

public class ExerciseEquipmentConfiguration : IEntityTypeConfiguration<ExerciseEquipment>
{
    public void Configure(EntityTypeBuilder<ExerciseEquipment> builder)
    {
        builder.ToTable("ExerciseEquipment");

        builder.HasKey(e => new { e.ExerciseId, e.EquipmentId });

        builder.Property(e => e.ExerciseId)
            .HasColumnType("char(36)");

        builder.Property(e => e.EquipmentId)
            .HasColumnType("char(36)");

        builder.HasOne(e => e.Exercise)
            .WithMany(ex => ex.Equipment)
            .HasForeignKey(e => e.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Equipment)
            .WithMany(eq => eq.Exercises)
            .HasForeignKey(e => e.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
