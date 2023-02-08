using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nest;
using OrchardCore;
using OrchardHeadlessCMS.Handler;
using OrchardHeadlessCMS.Models;
using System.Text.Json;

namespace OrchardHeadlessCMS.Pages
{
    public class EventModel : PageModel
    {
        [FromRoute]
        public string? Id { get; set; }

        private readonly ContentItemHandler _handler;
        public EventModel(ContentItemHandler handler)
        {
            _handler = handler;
        }

        public ItemContent ContentItem { get; set; } = new();

        public async Task OnGetAsync()
        {
            ContentItem = await _handler.GetSingleAsync(Id);
        }
    }
}
