using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore;
using OrchardHeadlessCMS.Models;
using System.Text.Json;

namespace OrchardHeadlessCMS.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IOrchardHelper OrchardHelper;
        private readonly JsonSerializerOptions _options;

        public IndexModel(ILogger<IndexModel> logger, IOrchardHelper orchardHelper)
        {
            _logger = logger;
            this.OrchardHelper = orchardHelper;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
        
        public Content? Data { get; set; } = new();
        public List<ContentItem?> Products { get; set; } = new();

        public async Task OnGetAsync()
        {
            var getHome = await OrchardHelper.GetRecentContentItemsByContentTypeAsync("Home", 1);
            var homeItems = getHome.FirstOrDefault();
            if(homeItems != null)
                Data = JsonSerializer.Deserialize<Content?>(homeItems?.Content.ToString(), _options);
            var getProducts = await OrchardHelper.GetRecentContentItemsByContentTypeAsync("Products", 10);
            var ProductsItems = getProducts.ToList();
            if (ProductsItems != null)
                foreach (var product in ProductsItems)
                {
                    Products.Add(new ContentItem
                    {
                        Author = product.Author,
                        ContentItemId = product.ContentItemId,
                        ContentTypeId = product.ContentType,
                        Owner = product.Owner,
                        CreatedUtc = product.CreatedUtc,
                        ModifiedUtc = product.ModifiedUtc,
                        PublishedUtc = product.PublishedUtc,
                        Content = JsonSerializer.Deserialize<Content?>(product?.Content.ToString(), _options)
                    });
                }
        }
    }
}