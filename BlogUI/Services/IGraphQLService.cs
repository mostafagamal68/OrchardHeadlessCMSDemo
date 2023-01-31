using BlogUI.Model;
namespace BlogUI.Services
{
    public interface IGraphQLService
    {
        List<BlogPost> BlogPosts { get; set; }
        ContentItem BlogPost { get; set; }
        Task GetAll();
        Task GetSingle(string? id);
    }
}
