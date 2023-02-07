using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nest;
using OrchardCore;
using OrchardHeadlessCMS.Models;
using System.Text.Json;

namespace OrchardHeadlessCMS.Pages
{
    public class BlogPostModel : PageModel
    {
        [FromRoute]
        public string? Id { get; set; }

        private readonly IOrchardHelper OrchardHelper;
        private readonly JsonSerializerOptions _options;
        public BlogPostModel(IOrchardHelper orchardHelper)
        {
            this.OrchardHelper = orchardHelper;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public ContentItem ContentItem { get; set; } = new();

        public async Task OnGetAsync()
        {
            var blogPost = await OrchardHelper.GetContentItemByIdAsync(Id);
            ContentItem.Author = blogPost?.Author;
            ContentItem.Content= JsonSerializer.Deserialize<Content?>(blogPost?.Content.ToString(), _options);
            ContentItem.DisplayText = blogPost?.DisplayText;
            ContentItem.Author = blogPost?.Author;
        }
    }
}
