using ResHub.Data;
using ResHub.Models;
using ResHub.Services.Interfaces;
using System;

namespace ResHub.Services.Implementations
{
    public class PostService: IPostService
    {
        private readonly ApplicationDbContext _context;

        public PostService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Post> AddPostAsync(Post post)
        {
            if (post == null) throw new ArgumentNullException(nameof(post));

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }
    }
}
