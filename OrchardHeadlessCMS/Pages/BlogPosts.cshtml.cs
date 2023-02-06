using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore;
using System.Text.Json;

namespace OrchardHeadlessCMS.Pages
{
    public class BlogPostsModel : PageModel
    {
        private readonly IOrchardHelper OrchardHelper;
        private readonly JsonSerializerOptions _options;
        public BlogPostsModel(IOrchardHelper orchardHelper)
        {
            this.OrchardHelper = orchardHelper;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public Content? Data { get; set; } = new();
        public async void OnGet()
        {
            var home = await OrchardHelper.GetRecentContentItemsByContentTypeAsync("Home", 1);
            var item = home.FirstOrDefault();
            Data = JsonSerializer.Deserialize<Content?>(item?.Content.ToString(), _options);
            //OrchardHelper.ConsoleLog(blogPost);
        }
    }
    public class Content
    {
        public Home? Home { get; set; }
        public MarkdownBodyPart? MarkdownBodyPart { get; set; }
        public TitlePart? TitlePart { get; set; }
    }
    public class ValueObject
    {
        public object? Value { get; set; }
    }
    public class Home
    {
        public ValueObject? Customers { get; set; }
        public ValueObject? Users { get; set; }
        public ValueObject? Modules { get; set; }
        public ValueObject? Features { get; set; }
    }
    public class MarkdownBodyPart
    {
        public string? Markdown { get; set; }
    }
    public class TitlePart
    {
        public string? Title { get; set; }
    }
    //public static partial class JsonSerializerExtensions
    //{
    //    public static T? DeserializeAnonymousType<T>(string json, T anonymousTypeObject, JsonSerializerOptions? options = default)
    //        => JsonSerializer.Deserialize<T>(json, options);

    //    public static ValueTask<TValue?> DeserializeAnonymousTypeAsync<TValue>(Stream stream, TValue anonymousTypeObject, JsonSerializerOptions? options = default, CancellationToken cancellationToken = default)
    //        => JsonSerializer.DeserializeAsync<TValue>(stream, options, cancellationToken); // Method to deserialize from a stream added for completeness
    //}
}
