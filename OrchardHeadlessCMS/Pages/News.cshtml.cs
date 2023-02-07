using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore;
using OrchardCore.XmlRpc.Controllers;
using OrchardHeadlessCMS.Models;
using System.Text.Json;

namespace OrchardHeadlessCMS.Pages
{
    public class NewsModel : PageModel
    {
        private readonly IOrchardHelper OrchardHelper;
        private readonly JsonSerializerOptions _options;
        public NewsModel(IOrchardHelper orchardHelper)
        {
            this.OrchardHelper = orchardHelper;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public List<ContentItem?> Data { get; set; } = new();
        public async Task OnGetAsync()
        {
            var getNewsAndEvents = await OrchardHelper.GetRecentContentItemsByContentTypeAsync("NewsAndEvents", 10);
            var NewsAndEvents = getNewsAndEvents.ToList();
            if (NewsAndEvents != null)
                foreach (var newevent in NewsAndEvents)
                {
                    Data.Add(new ContentItem
                    {
                        Author = newevent.Author,
                        ContentItemId = newevent.ContentItemId,
                        ContentTypeId  = newevent.ContentType,
                        Owner = newevent.Owner,
                        CreatedUtc = newevent.CreatedUtc,
                        ModifiedUtc = newevent.ModifiedUtc,
                        PublishedUtc = newevent.PublishedUtc,
                        Content = JsonSerializer.Deserialize<Content?>(newevent?.Content.ToString(), _options)
                    });
                }            
        }
    }
    
    //public static partial class JsonSerializerExtensions
    //{
    //    public static T? DeserializeAnonymousType<T>(string json, T anonymousTypeObject, JsonSerializerOptions? options = default)
    //        => JsonSerializer.Deserialize<T>(json, options);

    //    public static ValueTask<TValue?> DeserializeAnonymousTypeAsync<TValue>(Stream stream, TValue anonymousTypeObject, JsonSerializerOptions? options = default, CancellationToken cancellationToken = default)
    //        => JsonSerializer.DeserializeAsync<TValue>(stream, options, cancellationToken); // Method to deserialize from a stream added for completeness
    //}
}
