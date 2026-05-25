using Fithub.Platform.Domain.Workout;
using FitHub.Platform.Common.Repository;

namespace Fithub.Platform.Repositories.Workout;

public class ExerciseRepository(FithubDbContext context) : BaseRepository<Exercise>(context), IExerciseRepository
{
}
