using E_CommerceApp_Backend.RequestHelpers;
using System.Text.Json;

namespace E_CommerceApp_Backend.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse httpResponse , MetaData metaData)
        {
            var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase,};

            httpResponse.Headers.Add("Pagination", JsonSerializer.Serialize(metaData, options));
            httpResponse.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
