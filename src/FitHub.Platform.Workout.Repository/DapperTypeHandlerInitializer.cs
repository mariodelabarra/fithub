using Dapper;
using FitHub.Platform.Workout.Domain;

namespace FitHub.Platform.Workout.Repository
{
    public static class DapperTypeHandlerInitializer
    {
        public static void RegisterHandlers()
        {
            SqlMapper.AddTypeHandler(new IntArrayTypeHandler());
            SqlMapper.AddTypeHandler(new EnumArrayTypeHandler<MuscleGroup>());
        }
    }
}
