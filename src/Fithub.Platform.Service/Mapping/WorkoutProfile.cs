using AutoMapper;
using Fithub.Platform.Domain.Workout;
using Fithub.Platform.Domain.Workout.In;

namespace Fithub.Platform.Services.Mapping
{
    public class WorkoutProfile : Profile
    {
        public WorkoutProfile()
        {
            CreateMap<CreateExerciseIn, Exercise>();
            CreateMap<UpdateExerciseIn, Exercise>();
            // Use CreateMap... Etc.. here (Profile methods are the same as configuration methods)
        }
    }
}
