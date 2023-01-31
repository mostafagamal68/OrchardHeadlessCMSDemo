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
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                "CfDJ8KeAqotG6RVDi2jj42U3rR3S41kiT9i8Fmv-VMRY1apQ_UPqokMQm0C3T1a6BA0VUbLWa0dpxLq9V8GLx8ssz9YP-0k_KX3hxVkyM58pRpiEK8nRRCrR9xNVUH0BLDZDTKV1ar9Or6BGmRp0X7miDOOshR7jl_X9cEfyx1luqKrzOF8t-slkJm7x7UScoFgewhuIOkdHUu92dDoHYi-PwZzB4HO-qP3kYQJ_K0Z6Eje0XJK15QtyAoBNoqNQkJ0sgGR6dKpcnTXBb0tyEBQOLhPlV6KURYa7cbK3bmTAogAK_18O77D1wrLTAIIWBM1N2KhOl8cYM7zIAmNMhj0Gbys34quQxfzo2s6VanhWDlFxsJqLWjqphVc61z7hkGWAS3RINKrR0Gf2mX0ltIqMd5fcGUot3sCG8Hu0Exs_IgEt3RxZxLP4wEiJBdkMj7oc3utbDM0iiMSEaDncqLid2p6gN4EFc6Mx-YAVnHoHwj1WvuPM_Zi4-AMqVjR22FxHYFGftSsTaUFXRzHEzCfuMjeQF7K4wfJvAz5oY6LwAFOvp0y6W6UqMAXIUA20b4v9TDyWmQgO0Os74znUNZNxIdd2J64YxuHf5wbAkIHlSG1ad-o_fhvZkuR0ctkdhKSGc4ulw3beb-9ohWOuR2HtmigYoTezNvwRnZJXHNrDgU2-61ZSabKwAOotrlrwLo_oBf5K4kxSuwtP_mOVRvDu6rDFTPkwXZWxu69tXK0Es6_OQd7DfTRDb_jfQegmogrOLVyYxHwYuQshfB9PgG9qO1GaLc7rZdFQnvIGL2ITY9ng-BlNTOaw6iDEqvXtJNhYZEqukURVCTZ6-3HLeMkC5wc-nANiZ1J2HoRz2ZOXvgwqfOCQIv0ijDUVy1qTQlxBbg"
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
        }

        public async Task<Authorize?> GetAuthorizeAsync()
        {
            var data = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", "e0f660a2cf2a47babac40a4a8c24e7e0"),
                new KeyValuePair<string, string>("client_secret", "76945d3917a4456db5a41fc2949d6439"),
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            };

            var responseAuth = await _client.PostAsync(tokenUri, new FormUrlEncodedContent(data));
            if (responseAuth.StatusCode == HttpStatusCode.OK)
            {
                var responseBody = await responseAuth.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Authorize>(responseBody, _options);
            }
            else 
                return null;
        }
        public async Task<Query?> QueryAsync(string method, string requestBody)
        {
            var content = new StringContent(requestBody, Encoding.UTF8, "application/graphql");
            var response = await _client.PostAsync(uri, content);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync(); ;
                return JsonSerializer.Deserialize<Query>(responseBody, _options);
            }
            else
                return null;
        }
    }
}
