using Fithub.Platform.Domain.Workout;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fithub.Platform.Repositories.Configuration;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.ToTable("Exercises");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnType("char(36)")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(e => e.Description)
            .HasMaxLength(2000);

        builder.Property(e => e.Instructions)
            .HasMaxLength(2000);

        builder.Property(e => e.VideoUrl)
            .HasMaxLength(512);

        builder.Property(e => e.CreatedByUserId)
            .HasMaxLength(256);

        builder.Property(e => e.Type)
            .HasConversion<int>();

        builder.Property(e => e.DifficultyLevel)
            .HasConversion<int>();

        builder.Property(e => e.IsPublic)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedOn).IsRequired();
        builder.Property(e => e.ModifiedOn);

        builder.HasMany(e => e.MuscleGroups)
            .WithOne(m => m.Exercise)
            .HasForeignKey(m => m.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Categories)
            .WithOne(c => c.Exercise)
            .HasForeignKey(c => c.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Equipment)
            .WithOne(eq => eq.Exercise)
            .HasForeignKey(eq => eq.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
