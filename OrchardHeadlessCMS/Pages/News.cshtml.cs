using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardHeadlessCMS.Handler;
using OrchardHeadlessCMS.Models;

namespace OrchardHeadlessCMS.Pages
{
    public class NewsModel : PageModel
    {
        private readonly ContentItemHandler _handler;
        public NewsModel(ContentItemHandler handler)
        {
            _handler = handler;
        }

        public List<ItemContent>? Data { get; set; } = new();
        public ContentTypeDefinition ContentTypeDefinition { get; set; }

        public async Task OnGetAsync()
        {
            ContentTypeDefinition = _handler.GetTypeAsync("NewsAndEvents");
            Data = await _handler.GetListByTypeAsync("NewsAndEvents");
        }        
    }    
}
