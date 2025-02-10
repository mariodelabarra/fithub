using FitHub.Platform.Common.Repository;
using FitHub.Platform.Workout.Domain;
using Microsoft.Extensions.Configuration;

namespace FitHub.Platform.Workout.Repository
{
    public interface IExerciseRepository : IBaseRepository<Exercise>
    {

    }

    public class ExerciseRepository : BaseRepository<Exercise>, IExerciseRepository
    {
        public ExerciseRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
