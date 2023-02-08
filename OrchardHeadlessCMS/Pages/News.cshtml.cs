using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public async Task OnGetAsync()
        {
            Data = await _handler.GetListByTypeAsync("NewsAndEvents");
        }        
    }    
}
