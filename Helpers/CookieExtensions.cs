using System.Text.Json;

namespace eCommerceWeb.Helpers
{
    public static class CookieExtensions
    {
        public static void SetObject(this HttpResponse response, string key, object value)
        {
            var options = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(7),
                HttpOnly = true,
                IsEssential = true
            };
            var jsonString = JsonSerializer.Serialize(value);
            response.Cookies.Append(key, jsonString, options);
        }
        public static T? GetObject<T>(this HttpRequest request, string key)
        {
            if (request.Cookies.TryGetValue(key, out var jsonString))
            {
                return JsonSerializer.Deserialize<T>(jsonString);
            }
            return default;
        }
    }
}
