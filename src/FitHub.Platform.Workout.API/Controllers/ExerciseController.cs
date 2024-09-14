using FitHub.Platform.Workout.Domain;
using FitHub.Platform.Workout.Service;
using Microsoft.AspNetCore.Mvc;

namespace FitHub.Platform.Workout.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;

        public ExerciseController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var exercises = await _exerciseService.GetAllAsync();

            return Ok(exercises);
        }

        [HttpGet("{exerciseId}")]
        public async Task<IActionResult> GetById(string exerciseId)
        {
            var exercise = await _exerciseService.GetById(exerciseId);

            return Ok(exercise);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateExerciseIn createExerciseIn)
        {
            await _exerciseService.Create(createExerciseIn);

            return Ok("Exercise created successfully!");
        }
    }
}
