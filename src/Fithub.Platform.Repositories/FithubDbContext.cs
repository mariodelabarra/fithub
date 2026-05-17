using Fithub.Platform.Domain.Workout;
using Microsoft.EntityFrameworkCore;

namespace Fithub.Platform.Repositories;

public class FithubDbContext(DbContextOptions<FithubDbContext> options) : DbContext(options)
{
    // Exercise catalog
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<ExerciseCategory> ExerciseCategories => Set<ExerciseCategory>();
    public DbSet<Equipment> Equipment => Set<Equipment>();
    public DbSet<ExerciseMuscleGroup> ExerciseMuscleGroups => Set<ExerciseMuscleGroup>();
    public DbSet<ExerciseExerciseCategory> ExerciseExerciseCategories => Set<ExerciseExerciseCategory>();
    public DbSet<ExerciseEquipment> ExerciseEquipment => Set<ExerciseEquipment>();

    // Workout templates
    public DbSet<WorkoutTemplate> WorkoutTemplates => Set<WorkoutTemplate>();
    public DbSet<WorkoutDay> WorkoutDays => Set<WorkoutDay>();
    public DbSet<WorkoutExercise> WorkoutExercises => Set<WorkoutExercise>();
    public DbSet<WorkoutExerciseSet> WorkoutExerciseSets => Set<WorkoutExerciseSet>();

    // Session tracking
    public DbSet<WorkoutSession> WorkoutSessions => Set<WorkoutSession>();
    public DbSet<SessionExerciseLog> SessionExerciseLogs => Set<SessionExerciseLog>();
    public DbSet<SessionSetLog> SessionSetLogs => Set<SessionSetLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FithubDbContext).Assembly);
    }
}
