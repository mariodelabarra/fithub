using AutoMapper;
using FitHub.Platform.Common.Exceptions;
using FitHub.Platform.Common.Service;
using FitHub.Platform.Workout.Domain;
using FitHub.Platform.Workout.Repository;

namespace FitHub.Platform.Workout.Service
{
    public interface IExerciseService
    {
        Task<IEnumerable<Exercise>> GetAllAsync();
        Task<Exercise> GetById(string exerciseId);
        Task Create(CreateExerciseIn createExerciseIn);
        Task Delete(string exerciseId);
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

        public async Task<IEnumerable<Exercise>> GetAllAsync()
        {
            return await _exerciseRepository.GetAllAsync();
        }

        public async Task<Exercise> GetById(string exerciseId)
        {
            var exercise = await _exerciseRepository.GetByIdAsync(exerciseId);
            
            if(exercise is null)
            {
                throw new NotFoundException(exerciseId);
            }
            
            return exercise;
        }

        public async Task Create(CreateExerciseIn createExerciseIn)
        {
            await _validatorService.ValidateAndThrow(createExerciseIn);

            var exercise = _mapper.Map<Exercise>(createExerciseIn);

            await _exerciseRepository.AddAsync(exercise);
        }

        public async Task Delete(string exerciseId)
        {
            _ = await GetById(exerciseId);

            await _exerciseRepository.DeleteAsync(exerciseId);
        }
    }
}
