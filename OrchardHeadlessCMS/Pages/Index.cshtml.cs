using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardHeadlessCMS.Handler;
using OrchardHeadlessCMS.Models;

namespace OrchardHeadlessCMS.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ContentItemHandler _handler;
        public IndexModel(ILogger<IndexModel> logger, ContentItemHandler handler)
        {
            _logger = logger;
            _handler = handler;
        }
        
        public Content? Data { get; set; } = new();
        public List<ItemContent>? Products { get; set; } = new();

        public async Task OnGetAsync()
        {
            Data = await _handler.GetFirstByTypeAsync("Home");
            Products = await _handler.GetListByTypeAsync("Products");
        }
    }
}