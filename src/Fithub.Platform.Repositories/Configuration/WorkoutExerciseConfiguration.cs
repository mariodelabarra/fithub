using System.Text.Json;
using Fithub.Platform.Domain.Workout;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fithub.Platform.Repositories.Configuration;

public class WorkoutExerciseConfiguration : IEntityTypeConfiguration<WorkoutExercise>
{
    public void Configure(EntityTypeBuilder<WorkoutExercise> builder)
    {
        builder.ToTable("WorkoutExercises");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnType("char(36)")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Sets)
            .HasColumnType("json")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<int[]>(v, (JsonSerializerOptions?)null) ?? Array.Empty<int>()
            );

        builder.Property(e => e.Reps)
            .HasColumnType("json")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<int[]>(v, (JsonSerializerOptions?)null) ?? Array.Empty<int>()
            );

        builder.Property(e => e.CreatedOn).IsRequired();
        builder.Property(e => e.ModifiedOn);
    }
}
