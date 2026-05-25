using AutoMapper;
using Fithub.Platform.Domain.Workout;
using Fithub.Platform.Domain.Workout.In;
using Fithub.Platform.Repositories.Workout;
using FitHub.Platform.Common.Domain;
using FitHub.Platform.Common.Exceptions;
using FitHub.Platform.Common.Service;

namespace Fithub.Platform.Services.Workout;

public interface IExerciseService
{
    Task<IEnumerable<Exercise>> GetAllAsync();
    Task<PaginatedResult<Exercise>> GetPagedAsync(PageRequest pageRequest);
    Task<Exercise> GetById(Guid id);
    Task Create(CreateExerciseIn createExerciseIn);
    Task<bool> Update(Guid id, UpdateExerciseIn updateExerciseIn);
    Task Delete(Guid id);
}

public class ExerciseService(IExerciseRepository exerciseRepository, IMapper mapper, IValidatorService validatorService) : IExerciseService
{
    private readonly IExerciseRepository _exerciseRepository = exerciseRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IValidatorService _validatorService = validatorService;

    public async Task<IEnumerable<Exercise>> GetAllAsync()
        => await _exerciseRepository.GetAllAsync();

    public async Task<PaginatedResult<Exercise>> GetPagedAsync(PageRequest pageRequest)
        => await _exerciseRepository.GetPagedAsync(pageRequest with { SortBy = pageRequest.SortBy ?? "Name" });

    public async Task<Exercise> GetById(Guid id)
    {
        var exercise = await _exerciseRepository.GetByIdAsync(id);

        if (exercise is null)
            throw new NotFoundException(id.ToString());

        return exercise;
    }

    public async Task Create(CreateExerciseIn createExerciseIn)
    {
        await _validatorService.ValidateAndThrow(createExerciseIn);
        var exercise = _mapper.Map<Exercise>(createExerciseIn);
        await _exerciseRepository.InsertAsync(exercise);
    }

    public async Task<bool> Update(Guid id, UpdateExerciseIn updateExerciseIn)
    {
        await _validatorService.ValidateAndThrow(updateExerciseIn);
        var exercise = await GetById(id);
        _mapper.Map(updateExerciseIn, exercise);
        var success = await _exerciseRepository.UpdateAsync(exercise);
        return success > 0;
    }

    public async Task Delete(Guid id)
    {
        _ = await GetById(id);
        await _exerciseRepository.DeleteAsync(id);
    }
}
