using ResHub.Models;

namespace ResHub.Services.Interfaces
{
    public interface IPostService
    {
        Task<Post> AddPostAsync(Post post);
    }
}
