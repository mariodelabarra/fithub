using Dapper;
using System.Data;
using System.Text.Json;

namespace FitHub.Platform.Workout.Repository
{
    public class IntArrayTypeHandler : SqlMapper.TypeHandler<int[]>
    {
        public override void SetValue(IDbDataParameter parameter, int[] value)
        {
            // Convert the int array to JSON string for storage
            parameter.Value = JsonSerializer.Serialize(value);
            parameter.DbType = DbType.String;
        }

        public override int[] Parse(object value)
        {
            // Parse the JSON string back to an int array
            if (value is string jsonString)
            {
                try
                {
                    return JsonSerializer.Deserialize<int[]>(jsonString) ?? Array.Empty<int>();
                }
                catch
                {
                    return Array.Empty<int>(); // Fallback in case of error
                }
            }
            return Array.Empty<int>();
        }
    }

}
