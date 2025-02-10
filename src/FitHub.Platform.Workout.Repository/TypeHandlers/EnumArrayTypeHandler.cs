using Dapper;
using System.Data;
using System.Text.Json;

namespace FitHub.Platform.Workout.Repository
{
    public class EnumArrayTypeHandler<TEnum> : SqlMapper.TypeHandler<TEnum[]> where TEnum : struct, Enum
    {
        public override void SetValue(IDbDataParameter parameter, TEnum[] value)
        {
            // Serialize the enum array to JSON
            parameter.Value = JsonSerializer.Serialize(value);
            parameter.DbType = DbType.String;
        }

        public override TEnum[] Parse(object value)
        {
            // Deserialize the JSON string back to the enum array
            if (value is string jsonString)
            {
                try
                {
                    return JsonSerializer.Deserialize<TEnum[]>(jsonString) ?? Array.Empty<TEnum>();
                }
                catch
                {
                    return Array.Empty<TEnum>();
                }
            }
            return Array.Empty<TEnum>();
        }
    }

}
