namespace OrchardHeadlessCMS.Models
{
    public class ItemContent
    {
        public string? Author { get; set; }
        public string? DisplayText { get; set; }
        public string? Owner { get; set; }
        public string? ContentItemId { get; set; }
        public string? ContentType { get; set; }
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
        public MarkdownBodyField? MarkdownBodyPart { get; set; }
        public TitleField? TitlePart { get; set; }
        public LiquidPart? LiquidPart { get; set; }
    }
    public class CustomFields
    {
        public ValueObject? Customers { get; set; }
        public ValueObject? Users { get; set; }
        public ValueObject? Modules { get; set; }
        public ValueObject? Features { get; set; }
        public ValueObject? Date { get; set; }
        public TextObject? Type { get; set; }
        public Image? Image { get; set; }
    }
    public class TextObject
    {
        public string? Text { get; set; }
    }
    public class ValueObject
    {
        public object? Value { get; set; }
    }
    public class Image
    {
        public string[]? Paths { get; set; }
    }
    public class MarkdownBodyField
    {
        public string? Markdown { get; set; }
    }
    public class TitleField
    {
        public string? Title { get; set; }
    }
    public class LiquidPart
    {
        public string? Liquid { get; set; }
    }
}
