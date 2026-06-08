namespace HealthAxis.Api.Helpers
{
    public static class UserIdGenerator
    {
        public static string Next(string prefix, string lastUserId)
        {
            if (string.IsNullOrWhiteSpace(lastUserId)) return prefix + "00001";
            int number = int.Parse(lastUserId.Substring(1));
            return prefix + (number + 1).ToString("D5");
        }
    }
}
