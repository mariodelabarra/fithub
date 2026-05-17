using Fithub.Platform.Domain.Workout;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fithub.Platform.Repositories.Configuration;

public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> builder)
    {
        builder.ToTable("Equipment");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnType("char(36)")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(e => e.Name).IsUnique();

        builder.Property(e => e.CreatedOn).IsRequired();
        builder.Property(e => e.ModifiedOn);

        builder.HasData(
            new Equipment { Id = Guid.Parse("22222222-0000-0000-0000-000000000001"), CreatedOn = DateTime.UtcNow, Name = "Barbell" },
            new Equipment { Id = Guid.Parse("22222222-0000-0000-0000-000000000002"), CreatedOn = DateTime.UtcNow, Name = "Dumbbell" },
            new Equipment { Id = Guid.Parse("22222222-0000-0000-0000-000000000003"), CreatedOn = DateTime.UtcNow, Name = "Kettlebell" },
            new Equipment { Id = Guid.Parse("22222222-0000-0000-0000-000000000004"), CreatedOn = DateTime.UtcNow, Name = "Cable Machine" },
            new Equipment { Id = Guid.Parse("22222222-0000-0000-0000-000000000005"), CreatedOn = DateTime.UtcNow, Name = "Resistance Band" },
            new Equipment { Id = Guid.Parse("22222222-0000-0000-0000-000000000006"), CreatedOn = DateTime.UtcNow, Name = "Pull-up Bar" },
            new Equipment { Id = Guid.Parse("22222222-0000-0000-0000-000000000007"), CreatedOn = DateTime.UtcNow, Name = "Bench" },
            new Equipment { Id = Guid.Parse("22222222-0000-0000-0000-000000000008"), CreatedOn = DateTime.UtcNow, Name = "Bodyweight" },
            new Equipment { Id = Guid.Parse("22222222-0000-0000-0000-000000000009"), CreatedOn = DateTime.UtcNow, Name = "Treadmill" },
            new Equipment { Id = Guid.Parse("22222222-0000-0000-0000-000000000010"), CreatedOn = DateTime.UtcNow, Name = "Bicycle" },
            new Equipment { Id = Guid.Parse("22222222-0000-0000-0000-000000000011"), CreatedOn = DateTime.UtcNow, Name = "Rowing Machine" }
        );
    }
}
