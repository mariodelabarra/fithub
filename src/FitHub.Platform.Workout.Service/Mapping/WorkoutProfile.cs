using AutoMapper;
using FitHub.Platform.Workout.Domain;

namespace FitHub.Platform.Workout.Service.Mapping
{
    public class WorkoutProfile : Profile
    {
        public WorkoutProfile()
        {
            CreateMap<CreateExerciseIn, Exercise>();
            // Use CreateMap... Etc.. here (Profile methods are the same as configuration methods)
        }
    }
}
