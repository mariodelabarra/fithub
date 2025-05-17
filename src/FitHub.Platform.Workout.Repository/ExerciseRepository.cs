using FitHub.Platform.Common.Domain;
using FitHub.Platform.Common.Repository;
using FitHub.Platform.Workout.Domain;
using Microsoft.Extensions.Configuration;

namespace FitHub.Platform.Workout.Repository
{
    public interface IExerciseRepository : IBaseRepository<Exercise>
    {
        Task<PaginatedResult<Exercise>> GetExercisesPaginatedAsync(string orderBy, int? top, int? skip, string filter);
    }

    public class ExerciseRepository : BaseRepository<Exercise>, IExerciseRepository
    {
        protected override string TableName { get; set; } = $"{nameof(Exercise)}s";

        public ExerciseRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<PaginatedResult<Exercise>> GetExercisesPaginatedAsync(string orderBy, int? top, int? skip, string filter)
        {
            string baseQuery = $"SELECT Id, Name, Type, DifficultyLevel FROM {TableReference}";
            string? whereClause = string.IsNullOrEmpty(filter) ? null : filter;

            return await base.GetPaginatedAsync(
                baseQuery,
                map: reader => new Exercise
                {
                    Id = reader.GetInt32("Id").ToString(),
                    Name = reader.GetString("Name"),
                    Type = (ExerciseType)reader.GetInt32("Type"),
                    DifficultyLevel = (DifficultyLevel)reader.GetInt32("DifficultyLevel")
                },
                orderBy: orderBy,
                top: top,
                skip: skip,
                filter: filter
                );
        }
    }
}
