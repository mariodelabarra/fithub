using FitHub.Platform.Workout.Domain;
using FitHub.Platform.Workout.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

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
        public async Task<IActionResult> GetAll()
        {
            var exercises = await _exerciseService.GetAllAsync();

            return Ok(exercises.AsQueryable());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var exercise = await _exerciseService.GetById(id);

            return Ok(exercise);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateExerciseIn createExerciseIn)
        {
            await _exerciseService.Create(createExerciseIn);

            return Ok("Exercise created successfully!");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateExerciseIn updateExerciseIn)
        {
            var success = await _exerciseService.Update(id, updateExerciseIn);

            if(!success)
            {
                return NotFound("Exercise not found!");
            }

            return NoContent();
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(string id, [FromBody] )

        [HttpDelete("{exerciseId}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _exerciseService.Delete(id);

            return Ok("Exercise deleted successfully!");
        }
    }
}
