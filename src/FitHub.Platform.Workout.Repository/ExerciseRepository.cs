using FitHub.Platform.Common.Repository;
using FitHub.Platform.Workout.Domain;

namespace FitHub.Platform.Workout.Repository
{
    public interface IExerciseRepository : IBaseRepository<Exercise>
    {

    }

    public class ExerciseRepository : BaseRepository<Exercise>, IExerciseRepository
    {
    }
}
