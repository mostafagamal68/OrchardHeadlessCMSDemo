using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using BlogUI.Model;
using static System.Net.WebRequestMethods;

namespace BlogUI.Services
{
    public class GraphQLService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        private readonly string uri = "https://localhost:7022/api/graphql";
        private List<RequestHeader> requestHeaders = new List<RequestHeader>()
        {
            new RequestHeader() { Name = "Content-Type", Value = "application/graphql" },
        };
        public GraphQLService(HttpClient client)
        {
            _client = client;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "CfDJ8KeAqotG6RVDi2jj42U3rR3L_7ZKv-q5umGmuGtgcLlQvX-x3vaAI9e8yo9O29uQSzvenkfYulYy-BuLGOPekfsKsNwPwAI3zkXklDQieTHxmjnlk8HlrnwreMHBr1-MqWLPtCKYLV_tku1tvIRUnO9vMVJEHwtwRcQ6nexmN3mvUmMyF3r36SIuGrpw1jk3wMdcfNoNXa04JnaBacv_PsFoRrjxWzXEEKV-mGvyVW4UijPbUXZSotR_gTRDdbpPEY8D80YzI2jtB69q4lj4aVA33ywt2rv2J2nDDpCI7cLpxRPInmC_pFvoUUyQADhh7N8i9ytrma9quBh__kt3Bdc2QTQNbtbIASs_wHBAFLl0pmFWb_FtaGndiwg_zKD8pbqQu8gEe8bGzNbgVb4_9DoB1X1wkOFkmDjgyyIPor_938qCRABhMVg_ZLNNQkWC-w3s42CVN-7EHfpo43QHZAjiDq8QHukE-MznPCmESA7XZqziW55_r3_2--xXBVEMzo4I6e5JlrlePKU4V4CiROlFSjHrQXBe2KsY82ZS_FOlP39oMzWQl7xEkSiQWKjJj4xxqsV7e1EhEhedHcvdewsDRR2P9Xm4TMAExnf7Gyv1AIgUDoAeCfAKQnpI94jQ9ZUUhN515BeB5fuxVJJAIIctdohRgMkOHR_JKFmAJGfjFDABOaN4FzQXCHENQjC5aiKrYxYogcMfKpQsBu-kICyvt1yC9oMPMuVRDkssX_Gm5rANZHL5SFSoIDj1xlaW_cf8sEUmajmZtJrl49CWjq5wKKj8eMj3LInnBv4ANvNuu8SliFdDXtqpqchhEkiMl77Ft-xYZ3p9ZXaRkp37cQ6IkkKtMDvP3cxYx_OvdIQffeqcv6us5Pp3kjuKfFTFBA");
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
        public async Task<Query?> GetAll()
        => await QueryAsync("POST", @"query MyQuery {
          blogPost(orderBy: {createdUtc: DESC}) {
            displayText
            markdownBody {
              html
              markdown
            }
            author
            contentItemId
          }
        }");

        public async Task<Query?> GetSingle(string? id)
        => await QueryAsync("POST", @"query MyQuery {
            contentItem(contentItemId: ""4bzhr5qkx3wp17jv3k2ansf6dx"") {
                contentItemId
                contentType
                author
                displayText
            }
        }");

        private async Task<Query?> QueryAsync(string method, string requestBody)
        {
            var requestMessage = new HttpRequestMessage()
            {
                Method = new HttpMethod(method),
                RequestUri = new Uri(uri),
                Content = string.IsNullOrEmpty(requestBody) ? null : new StringContent(requestBody)
            };
            foreach (var header in requestHeaders)
            {
                // StringContent automatically adds its own Content-Type header with default value "text/plain"
                // If the developer is trying to specify a content type explicitly, we need to replace the default value,
                // rather than adding a second Content-Type header.
                if (header.Name.Equals("Content-Type", StringComparison.OrdinalIgnoreCase) && requestMessage.Content != null)
                {
                    requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(header.Value);
                    continue;
                }

                if (!requestMessage.Headers.TryAddWithoutValidation(header.Name, header.Value))
                {
                    requestMessage.Content?.Headers.TryAddWithoutValidation(header.Name, header.Value);
                }
            }
            var response = await _client.SendAsync(requestMessage);
            //if (response.StatusCode == System.Net.HttpStatusCode.OK)            
            var responseBody = await response.Content.ReadAsStringAsync(); ;
            var result = JsonSerializer.Deserialize<Query>(responseBody, _options);
            return result;
            //}
        }
    }
    public class RequestHeader
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
