using FluentValidation;

namespace FitHub.Platform.Workout.Domain
{
    public class CreateExerciseIn
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CreateExerciseInValidator : AbstractValidator<CreateExerciseIn>
    {
        public CreateExerciseInValidator()
        {
            RuleFor(exercise => exercise.Name).NotEmpty();

            RuleFor(exercise => exercise.Description)
                .NotEmpty()
                .MinimumLength(15);
        }
    }
}
