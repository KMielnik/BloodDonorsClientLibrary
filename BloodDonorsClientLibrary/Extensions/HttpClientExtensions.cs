using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BloodDonorsClientLibrary.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PostJsonAsync(this HttpClient client,string requestUri,HttpContent content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Content = content;
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            return await client.SendAsync(request);
        }
    }
}