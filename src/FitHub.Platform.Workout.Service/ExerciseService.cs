using AutoMapper;
using FitHub.Platform.Common.Exceptions;
using FitHub.Platform.Workout.Domain;
using FitHub.Platform.Workout.Repository;

namespace FitHub.Platform.Workout.Service
{
    public interface IExerciseService
    {
        Task<IEnumerable<Exercise>> GetAllAsync();
        Task<Exercise> GetById(string exerciseId);
        Task Create(CreateExerciseIn createExerciseIn);
    }

    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _exerciseRepository;

        private readonly IMapper _mapper;

        public ExerciseService(IExerciseRepository exerciseRepository, IMapper mapper)
        {
            _exerciseRepository = exerciseRepository;

            _mapper = mapper;
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
            //var createTaskInValidator = new CreateTaskInValidator();
            //createTaskInValidator.Validate(createTaskIn);

            var exercise = _mapper.Map<Exercise>(createExerciseIn);

            await _exerciseRepository.AddAsync(exercise);
        }
    }
}
