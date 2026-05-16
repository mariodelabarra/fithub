using Fithub.Platform.Domain.Workout;
using Microsoft.EntityFrameworkCore;

namespace Fithub.Platform.Repositories.Workout;

public class ExerciseRepository(FithubDbContext context) : IExerciseRepository
{
    public async Task<IEnumerable<Exercise>> GetAllAsync()
        => await context.Exercises.AsNoTracking().ToListAsync();

    public async Task<Exercise?> GetByIdAsync(Guid id)
        => await context.Exercises.FindAsync(id);

    public async Task<int> InsertAsync(Exercise entity)
    {
        context.Exercises.Add(entity);
        return await context.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(Exercise entity)
    {
        entity.ModifiedOn = DateTime.UtcNow;
        context.Exercises.Update(entity);
        return await context.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        var exercise = await context.Exercises.FindAsync(id);
        if (exercise is null) return 0;

        context.Exercises.Remove(exercise);
        return await context.SaveChangesAsync();
    }
}
