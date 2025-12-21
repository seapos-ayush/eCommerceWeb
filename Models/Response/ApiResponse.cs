using System.Text.Json;

namespace eCommerceWeb.Models.Response
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; }
        public JsonElement Data { get; set; }
        public List<string> Errors { get; set; }
    }

    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }
    }


}
