using Fithub.Platform.Domain.Workout;
using Microsoft.EntityFrameworkCore;

namespace Fithub.Platform.Repositories;

public class FithubDbContext(DbContextOptions<FithubDbContext> options) : DbContext(options)
{
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<WorkoutExercise> WorkoutExercises => Set<WorkoutExercise>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FithubDbContext).Assembly);
    }
}
