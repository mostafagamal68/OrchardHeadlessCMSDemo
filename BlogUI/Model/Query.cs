namespace BlogUI.Model
{
    public class Query
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public ContentItem contentItem { get; set; }
        public List<BlogPost> blogPost { get; set; }
    }

    public class BlogPost
    {
        public string contentItemId { get; set; }
        public MarkDownBody markdownBody { get; set; }
        public string author { get; set; }
        public string displayText { get; set; }
        //public string? createdUtc { get; set; }
    }

    public class MarkDownBody
    {
        public string html { get; set; }
        public string markdown { get; set; }
    }

    public class ContentItem
    {
        public MarkDownBody markdownBody { get; set; }
        public string contentType { get; set; }
        public string author { get; set; }
        public string displayText { get; set; }
    }
}
