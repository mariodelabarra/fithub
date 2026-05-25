using Fithub.Platform.Domain.Workout.In;
using Fithub.Platform.Services.Workout;
using FitHub.Platform.Common.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Fithub.Platform.API.Controllers.Workout;

[Route("api/[controller]")]
[ApiController]
public class ExerciseController : ControllerBase
{
    private readonly IExerciseService _exerciseService;

    public ExerciseController(IExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] PageRequest pageRequest)
    {
        var result = await _exerciseService.GetPagedAsync(pageRequest);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var exercise = await _exerciseService.GetById(id);
        return Ok(exercise);
    }

    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreateExerciseIn createExerciseIn)
    {
        await _exerciseService.Create(createExerciseIn);
        return Ok("Exercise created successfully!");
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateExerciseIn updateExerciseIn)
    {
        var success = await _exerciseService.Update(id, updateExerciseIn);

        if (!success)
            return NotFound("Exercise not found!");

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _exerciseService.Delete(id);
        return Ok("Exercise deleted successfully!");
    }
}
