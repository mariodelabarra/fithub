using Fithub.Platform.Domain.Workout;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fithub.Platform.Repositories.Configuration;

public class WorkoutDayConfiguration : IEntityTypeConfiguration<WorkoutDay>
{
    public void Configure(EntityTypeBuilder<WorkoutDay> builder)
    {
        builder.ToTable("WorkoutDays");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnType("char(36)")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.WorkoutTemplateId)
            .HasColumnType("char(36)")
            .IsRequired();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(e => e.Notes)
            .HasMaxLength(1000);

        builder.Property(e => e.CreatedOn).IsRequired();
        builder.Property(e => e.ModifiedOn);

        builder.HasIndex(e => e.WorkoutTemplateId);
    }
}
