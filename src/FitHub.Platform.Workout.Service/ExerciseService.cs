using AutoMapper;
using FitHub.Platform.Common.Domain;
using FitHub.Platform.Common.Exceptions;
using FitHub.Platform.Common.Service;
using FitHub.Platform.Workout.Domain;
using FitHub.Platform.Workout.Repository;

namespace FitHub.Platform.Workout.Service
{
    public interface IExerciseService
    {
        Task<PaginatedResult<Exercise>> GetPaginatedAsync(PaginatedRequestIn requestIn);
        Task<IEnumerable<Exercise>> GetAllAsync();
        Task<Exercise> GetById(int id);
        Task Create(CreateExerciseIn createExerciseIn);
        Task<bool> Update(int id, UpdateExerciseIn updateExerciseIn);
        Task Delete(int id);
    }

    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _exerciseRepository;

        private readonly IMapper _mapper;
        private readonly IValidatorService _validatorService;

        public ExerciseService(IExerciseRepository exerciseRepository, IMapper mapper, IValidatorService validatorService)
        {
            _exerciseRepository = exerciseRepository;

            _mapper = mapper;
            _validatorService = validatorService;
        }
        
        public async Task<PaginatedResult<Exercise>> GetPaginatedAsync(PaginatedRequestIn requestIn)
        {
            var result = await _exerciseRepository.GetExercisesPaginatedAsync(requestIn.OrderBy, requestIn.Top, requestIn.Skip, requestIn.Filter);

            return result;
        }

        public async Task<IEnumerable<Exercise>> GetAllAsync()
        {
            return await _exerciseRepository.GetAllAsync();
        }

        public async Task<Exercise> GetById(int id)
        {
            var exercise = await _exerciseRepository.GetByIdAsync(id);
            
            if(exercise is null)
            {
                throw new NotFoundException(id.ToString());
            }
            
            return exercise;
        }

        public async Task Create(CreateExerciseIn createExerciseIn)
        {
            await _validatorService.ValidateAndThrow(createExerciseIn);

            var exercise = _mapper.Map<Exercise>(createExerciseIn);

            await _exerciseRepository.InsertAsync(exercise);
        }

        public async Task<bool> Update(int id, UpdateExerciseIn updateExerciseIn)
        {
            await _validatorService.ValidateAndThrow(updateExerciseIn);

            var exercise = await GetById(id);

            _mapper.Map(updateExerciseIn, exercise);

            var success = await _exerciseRepository.UpdateAsync(exercise);

            return success > 0;
        }

        public async Task Delete(int id)
        {
            _ = await GetById(id);

            await _exerciseRepository.DeleteAsync(id);
        }
    }
}
