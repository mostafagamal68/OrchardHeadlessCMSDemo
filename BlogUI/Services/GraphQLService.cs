using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Text.Json;
using BlogUI.Model;

namespace BlogUI.Services
{
    public class GraphQLService : IGraphQLService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        private readonly string uri = "https://localhost:7022/api/graphql";
        private readonly string tokenUri = "https://localhost:7022/connect/token";
        public GraphQLService(HttpClient client)
        {
            _client = client;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public List<BlogPost> BlogPosts { get; set; } = new();
        public ContentItem BlogPost { get; set; } = new();
        public async Task GetAll()
        {
            var result = await QueryAsync(@"
                query MyQuery {
                  blogPost(orderBy: {createdUtc: DESC}) {
                    displayText
                    markdownBody {
                      html
                      markdown
                    }
                    author
                    contentItemId
                    contentType
                  }
                }");
            if (result != null)
                BlogPosts = result.data.blogPost;
        }

        public async Task GetSingle(string? id)
        {
            var result = await QueryAsync($$"""
                query MyQuery {
                  contentItem(contentItemId: "{{id}}") {
                    ... on BlogPost {
                      displayText
                      markdownBody {
                        html
                        markdown
                      }
                      contentType
                      author
                    }
                  }
                }
                """);
            if (result != null)
                BlogPost = result.data.contentItem;
        }

        private async Task<Query?> QueryAsync(string requestBody)
        {
            var data = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", "e0f660a2cf2a47babac40a4a8c24e7e0"),
                new KeyValuePair<string, string>("client_secret", "76945d3917a4456db5a41fc2949d6439"),
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            };

            HttpResponseMessage? responseAuth = await _client.PostAsync(tokenUri, new FormUrlEncodedContent(data));
            string? responseAuthBody = await responseAuth.Content.ReadAsStringAsync();
            Authorize? auth = JsonSerializer.Deserialize<Authorize>(responseAuthBody, _options);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(auth.token_type, auth.access_token);

            var content = new StringContent(requestBody, Encoding.UTF8, "application/graphql");
            HttpResponseMessage? response = await _client.PostAsync(uri, content);
            string? responseBody = await response.Content.ReadAsStringAsync(); ;
            return JsonSerializer.Deserialize<Query>(responseBody, _options);
        }
    }
}
