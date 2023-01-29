using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using BlogUI.Model;

namespace BlogUI.Services
{
    public class GraphQLService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        private readonly string uri = "https://localhost:7022/api/graphql";

        public GraphQLService(HttpClient client)
        {
            _client = client;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "CfDJ8KeAqotG6RVDi2jj42U3rR0A8LfJpXLhlg-JIIkXDzXRXjYcBse3oT4jmy2FtgSBen9BfCAzqmWQJiM7bVuO5n8-lmZUSN-BUoNGrB5uyYi6jG5ZeFXylyxc1471JtR42EBEEptxdjNF1g760mkdLSjSd61ACLi6ESGli3CXSonGHErBIYW68ZB5kPCAw4KeRPp7nKHXMUfjnTA1_i-FhmHIdzZORZBsb0ySq-rl0c3d8yM9wCc-LmYjpf4JkQ8e7MmTnvmDY2fQ3Ab-5jb6np970c48EJAOFuXiAS5ELsLxKqCqcUHvuUPlRQCBYIB41EeYena2nW4oCVoRDykEvRgoIGyOqhbqkxOVR84rf86SuHQwSeKsggebj4ptz3qPZEU0hD3iHKh1zpaxK6YJfIwZPIzhtLWB5YDEMcTEtoBisw0MUJdTc2R1iW25xVty6Hr3XiNHSAudCPbM1yjVH0MtrB6-mKHBuvqQk3rON_hCvaemUVK9l00eKmOvmp7dTNs5vsvnOnBQ6L6wSg8O5G4QWl4eZpeyOt3vXghcrUhHjQ9F0q6K_EzQmaW6hlLpC_6ZiPMnUpjPu-O4ECAL4t0mOpcfFBYRAFcnjSBmL5bIg5E-YIzdr36-8lxOdzK1Zohwny7xxq8dkymY-lalFyoU9HR2lV-gWhQhVWj1mOjGrSQSroHzi12RNZk4GcUFHNPubPi0WE5uQXVPZv43vo6mDiaCST0nlFFs6G2A4bEuTQquohgf4x91kUcQoJRyc5p-apm8-_wyFF1X9vO0K9OrNcj2OTT5fUKj-RjMn1crUmUg0ckig_BcAyTvviv0gZoCtYmnCtUezGQql8cspabkHXWtEJMZn4XNAA1N3c24QmNHkpq-PheGPoWW8Zs6Ig");
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
        public async Task<Query> GetAll(string requestBody)
        {
            var response = await _client.SendAsync(new HttpRequestMessage()
            {
                Method = new HttpMethod("POST"),
                RequestUri = new Uri(uri),
                Content = string.IsNullOrEmpty(requestBody) ? null : new StringContent(requestBody, Encoding.UTF8, "application/graphql")
            });
            //if (response.StatusCode == System.Net.HttpStatusCode.OK)            
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Query>(responseBody, _options);
            return result;
            //}
        }
        public async Task<Query> GetSingle(string requestBody)
        {
            var response = await _client.SendAsync(new HttpRequestMessage()
            {
                Method = new HttpMethod("POST"),
                RequestUri = new Uri(uri),
                Content = string.IsNullOrEmpty(requestBody) ? null : new StringContent(requestBody, Encoding.UTF8, "application/graphql")
            });
            //if (response.StatusCode == System.Net.HttpStatusCode.OK)            
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Query>(responseBody, _options);
            return result;
            //}
        }
    }
}
