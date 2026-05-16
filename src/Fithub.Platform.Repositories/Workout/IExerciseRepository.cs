using Fithub.Platform.Domain.Workout;

namespace Fithub.Platform.Repositories.Workout;

public interface IExerciseRepository
{
    Task<IEnumerable<Exercise>> GetAllAsync();
    Task<Exercise?> GetByIdAsync(Guid id);
    Task<int> InsertAsync(Exercise entity);
    Task<int> UpdateAsync(Exercise entity);
    Task<int> DeleteAsync(Guid id);
}
