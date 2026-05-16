using System.Text.Json;
using Fithub.Platform.Domain.Workout;
using Fithub.Platform.Domain.Workout.Enums;
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

        builder.Property(e => e.Categories)
            .HasMaxLength(1000);

        builder.Property(e => e.Type)
            .HasConversion<int>();

        builder.Property(e => e.DifficultyLevel)
            .HasConversion<int>();

        builder.Property(e => e.MuscleGroups)
            .HasColumnType("json")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<MuscleGroup[]>(v, (JsonSerializerOptions?)null) ?? Array.Empty<MuscleGroup>()
            );

        builder.Property(e => e.CreatedOn).IsRequired();
        builder.Property(e => e.ModifiedOn);
    }
}
