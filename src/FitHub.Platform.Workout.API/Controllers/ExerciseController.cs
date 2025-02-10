using FitHub.Platform.Workout.Domain;
using FitHub.Platform.Workout.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using MongoDB.AspNetCore.OData;

namespace FitHub.Platform.Workout.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ODataController
    {
        private readonly IExerciseService _exerciseService;

        public ExerciseController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        [HttpGet]
        [MongoEnableQuery]
        public async Task<IActionResult> GetAll()
        {
            var exercises = await _exerciseService.GetAllAsync();

            return Ok(exercises.AsQueryable());
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

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(string id, [FromBody] )

        [HttpDelete("{exerciseId}")]
        public async Task<IActionResult> Delete(string exerciseId)
        {
            await _exerciseService.Delete(exerciseId);

            return Ok("Exercise deleted successfully!");
        }
    }
}
