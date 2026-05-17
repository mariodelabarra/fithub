using AutoMapper;
using Fithub.Platform.Domain.Workout;
using Fithub.Platform.Domain.Workout.In;

namespace Fithub.Platform.Services.Mapping;

public class WorkoutProfile : Profile
{
    public WorkoutProfile()
    {
        CreateMap<CreateExerciseIn, Exercise>()
            .ForMember(dest => dest.MuscleGroups, opt => opt.MapFrom(src =>
                src.MuscleGroups.Select(m => new ExerciseMuscleGroup
                {
                    MuscleGroup = m.MuscleGroup,
                    IsPrimary = m.IsPrimary
                }).ToList()))
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src =>
                src.CategoryIds.Select(id => new ExerciseExerciseCategory
                {
                    ExerciseCategoryId = id
                }).ToList()))
            .ForMember(dest => dest.Equipment, opt => opt.MapFrom(src =>
                src.EquipmentIds.Select(id => new ExerciseEquipment
                {
                    EquipmentId = id
                }).ToList()));

        CreateMap<UpdateExerciseIn, Exercise>()
            .ForMember(dest => dest.MuscleGroups, opt => opt.MapFrom(src =>
                src.MuscleGroups.Select(m => new ExerciseMuscleGroup
                {
                    MuscleGroup = m.MuscleGroup,
                    IsPrimary = m.IsPrimary
                }).ToList()))
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src =>
                src.CategoryIds.Select(id => new ExerciseExerciseCategory
                {
                    ExerciseCategoryId = id
                }).ToList()))
            .ForMember(dest => dest.Equipment, opt => opt.MapFrom(src =>
                src.EquipmentIds.Select(id => new ExerciseEquipment
                {
                    EquipmentId = id
                }).ToList()));
    }
}
