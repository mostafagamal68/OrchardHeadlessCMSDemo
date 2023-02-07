namespace OrchardHeadlessCMS.Models
{
    public class ContentItem
    {
        public string? Author { get; set; }
        public string? DisplayText { get; set; }
        public string? Owner { get; set; }
        public string? ContentItemId { get; set; }
        public string? ContentTypeId { get; set; }
        public DateTime? CreatedUtc { get; set; }
        public DateTime? ModifiedUtc { get; set; }
        public DateTime? PublishedUtc { get; set; }
        public Content? Content { get; set; }
    }
    public class Content
    {
        public CustomFields? Home { get; set; }
        public CustomFields? Products { get; set; }
        public CustomFields? NewsAndEvents { get; set; }
        public MarkdownBodyPart? MarkdownBodyPart { get; set; }
        public TitlePart? TitlePart { get; set; }
    }
    public class CustomFields
    {
        public ValueObject? Customers { get; set; }
        public ValueObject? Users { get; set; }
        public ValueObject? Modules { get; set; }
        public ValueObject? Features { get; set; }
        public ValueObject? Date { get; set; }
        public ValueObject? Type { get; set; }
        public Image? Image { get; set; }
    }
    public class ValueObject
    {
        public object? Value { get; set; }
    }
    public class Image
    {
        public string[]? Paths { get; set; }
    }
    public class MarkdownBodyPart
    {
        public string? Markdown { get; set; }
    }
    public class TitlePart
    {
        public string? Title { get; set; }
    }
}
