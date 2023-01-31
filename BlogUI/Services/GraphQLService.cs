using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using BlogUI.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlogUI.Services
{
    public class GraphQLService : IGraphQLService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        private readonly string uri = "https://localhost:7022/api/graphql";
        public GraphQLService(HttpClient client)
        {
            _client = client;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                "CfDJ8KeAqotG6RVDi2jj42U3rR2r_reGmy5CegwkVwzydYd3z3RMcs6UMMv9bbw1NhO6kUCvtSXddpupDmnn_ij8WtiQ-AumgcU1QUisbXUU3BS5deXRNSXkj8Zs08_Zq0CLvusx_-2WjH3nfk15_MaU86Hxx-kzU8bpU16oVSaGAua7LhcNcvt_hE0onVtExij1rwAjsPZr3nH-_nRsckiACEfRbW9q7yg9A4Jf68YgQUqKh_3ETy79Dk4AyI14e8rMn5z6pn6bU6qrMsfEJSYJTEnzXwZsDLstn6SmJN8sgqmwVhW2nH_s8k0ySNzgN-pWvpddGK6J9w4YloqBeGPZl1WS0vtWfr3eR2P627UxDFEjn4vKa5U1h6h8TnNCV9tRjsy5zeX5grBTFp5msOvoCXtq67h_ZpSDC5vPLscbW8hPHnhinzwt6XWxDS1PEX4WiIN5SG5qdqRiIYwVbIpAy_kixm9jqILSZOEjzI7L4cdeQW03BxvkdriMHKJZs4WVBPOpmnvirlTXym1d5BAyLHcr2TTyHoMCejZ4Nnp0xWeCGNqC6qu0qgT31NKXMUb-KLow9jc4gSWod-7NlZuYlHuDpmANLcWQLsd-AIaVigWQAaFmrW7aBaZsWF2D6gNLZvpHx5w7CTmo4CVkzL35ttY7dBzpJuoVYvuWBDyHM4li8ezlvWzOvWl-LInQMkbmWfVOvN1IHwSKSu9WMyw5mRHYG8acQiCt2kBZtkYHYZv5LMgaholI1Dj4UiN-fVipbaE8R6ZSA6w2pmU05G2XtNk5-jgPykOKuZXyxxcwP4t3RzolpyzvZWy1mI4HInRtHPfSEnaVGhbVYvDatbDqBMylvl64dcYO4PO6Mi2-8Ch-mKDjbjYDweGJDgEgz7itVQ"
            );
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public List<BlogPost> BlogPosts { get; set; } = new();
        public ContentItem BlogPost { get; set; } = new();
        public async Task GetAll()
        {
            var result = await QueryAsync("POST", @"
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
            var result = await QueryAsync("POST", $$"""
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
            //else return null;

        }

        public async Task<Query?> QueryAsync(string method, string requestBody)
        {
            var content = new StringContent(requestBody, Encoding.UTF8, "application/graphql");
            var response = await _client.PostAsync(uri, content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync(); ;
                return JsonSerializer.Deserialize<Query>(responseBody, _options);
            }
            else
                return null;
        }
    }
}
