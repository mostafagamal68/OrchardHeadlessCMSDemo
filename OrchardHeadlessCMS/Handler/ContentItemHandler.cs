using OrchardCore;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.Flows.Models;
using OrchardCore.Markdown.Models;
using OrchardCore.Title.Models;
using OrchardHeadlessCMS.Models;
using System.Text.Json;

namespace OrchardHeadlessCMS.Handler
{
    public class ContentItemHandler
    {
        private readonly JsonSerializerOptions _options;
        private readonly IOrchardHelper _orchardHelper;
        private readonly IContentManager? _contentManager;
        private readonly IContentItemIdGenerator? _contentItemIdGenerator;
        private readonly IContentDefinitionManager? _contentDefinitionManager;
        public ContentItemHandler(IOrchardHelper orchardHelper)
        {
            _orchardHelper = orchardHelper;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _contentManager = _orchardHelper.HttpContext.RequestServices.GetService<IContentManager>();
            _contentItemIdGenerator = _orchardHelper.HttpContext.RequestServices.GetService<IContentItemIdGenerator>();
            _contentDefinitionManager = _orchardHelper.HttpContext.RequestServices.GetService<IContentDefinitionManager>();
        }

        private async Task<ContentItem?> BuildContentItem(string? type, string? summary, string? text, string? author)
        {
            var contentItem = await _contentManager.NewAsync(type);
            contentItem.Author = author; contentItem.Owner = ""; contentItem.DisplayText = summary;
            contentItem.CreatedUtc = DateTime.UtcNow; contentItem.ModifiedUtc = DateTime.UtcNow; contentItem.PublishedUtc = DateTime.UtcNow;
            contentItem.Latest = true; contentItem.Published = true;
            var titlePart = contentItem.As<TitlePart>();
            titlePart.Title = summary;
            titlePart.Apply();
            var markdownPart = contentItem.As<MarkdownBodyPart>();
            markdownPart.Markdown = text;
            markdownPart.Apply();
            return contentItem;
        }

        private ItemContent BuildItemContent(ContentItem? contentItem) => new ItemContent
        {
            Id = contentItem?.Id,
            Author = contentItem?.Author,
            DisplayText = contentItem?.DisplayText,
            ContentItemId = contentItem?.ContentItemId,
            ContentType = contentItem?.ContentType,
            Owner = contentItem?.Owner,
            CreatedUtc = contentItem?.CreatedUtc,
            ModifiedUtc = contentItem?.ModifiedUtc,
            PublishedUtc = contentItem?.PublishedUtc,
            Content = JsonSerializer.Deserialize<Content?>(contentItem?.Content.ToString(), _options)
        };

        public async Task PostComment(string? id, string? type, string? summary, string? text, string? author)
        {
            var contentItem = await _orchardHelper.GetContentItemByIdAsync(id);
            var newContentItem = await BuildContentItem(type, summary, text, author);
            contentItem.Alter<BagPart>("CommentsForNews", x =>
            {
                x.ContentItems.Add(newContentItem);
            });
            await _contentManager.UpdateAsync(contentItem);
        }
        public async Task PostReview(string? type, string? summary, string? text, string? author, decimal? stars)
        {
            var newContentItem = await BuildContentItem(type, summary, text, author);
            newContentItem.Alter<ContentPart>(type, x =>
            {
                x.Alter<NumericField>("Stars", c => c.Value = stars);
            });
            await _contentManager.CreateAsync(newContentItem);
        }

        public async Task<ItemContent> GetSingleAsync(string? Id)
        {
            var contentItem = await _orchardHelper.GetContentItemByIdAsync(Id);
            return BuildItemContent(contentItem);
        }

        public async Task<List<ItemContent>?> GetListByTypeAsync(string? type)
        {
            var query = await _orchardHelper.QueryContentItemsAsync(q => q.Where(c => c.ContentType == type && c.Published == true));
            var roundedCount = Math.Round(query.Count() / 10M, MidpointRounding.ToPositiveInfinity);
            var result = query.ToList();
            if (result != null)
            {
                var ContentItems = new List<ItemContent>();
                foreach (var contentItem in result)
                    ContentItems.Add(BuildItemContent(contentItem));

                return ContentItems;
            }
            return null;
        }

        public async Task<Content?> GetFirstByTypeAsync(string? type)
        {
            var getContentItem = await _orchardHelper.GetRecentContentItemsByContentTypeAsync(type, 1);
            var contentItem = getContentItem.FirstOrDefault();
            if (contentItem != null)
                return JsonSerializer.Deserialize<Content?>(contentItem?.Content.ToString(), _options);
            return null;
        }
        public ContentTypeDefinition GetTypeAsync(string? type) => _contentDefinitionManager.GetTypeDefinition(type);

        //public static partial class JsonSerializerExtensions
        //{
        //    public static T? DeserializeAnonymousType<T>(string json, T anonymousTypeObject, JsonSerializerOptions? options = default)
        //        => JsonSerializer.Deserialize<T>(json, options);

        //    public static ValueTask<TValue?> DeserializeAnonymousTypeAsync<TValue>(Stream stream, TValue anonymousTypeObject, JsonSerializerOptions? options = default, CancellationToken cancellationToken = default)
        //        => JsonSerializer.DeserializeAsync<TValue>(stream, options, cancellationToken); // Method to deserialize from a stream added for completeness
        //}
    }
}
