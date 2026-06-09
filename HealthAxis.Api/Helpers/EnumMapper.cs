using System;

namespace HealthAxis.Api.Helpers
{
    public static class EnumMapper
    {
        public static T ParseEnum<T>(string value) where T : struct
        {
            T parsed;
            return Enum.TryParse(value, true, out parsed) ? parsed : default(T);
        }
    }
}
