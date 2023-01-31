using BlogUI.Model;
using System.Text.Json;
using System.Text;
using System;

namespace BlogUI.Services
{
    public interface IGraphQLService
    {
        List<BlogPost> BlogPosts { get; set; }
        ContentItem BlogPost { get; set; }
        Task GetAll();

        Task GetSingle(string? id);

        Task<Query?> QueryAsync(string method, string requestBody);
    }
}
