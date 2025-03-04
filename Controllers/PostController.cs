using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResHub.Models;
using ResHub.Services.Interfaces;

namespace ResHub.Controllers
{
    [Route("api/posts")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPost([FromBody] Post post)
        {
            if (post == null || string.IsNullOrWhiteSpace(post.Content))
            {
                return BadRequest("Invalid post data.");
            }

            var createdPost = await _postService.AddPostAsync(post);
            return CreatedAtAction(nameof(AddPost), new { id = createdPost.Id }, createdPost);
        }
    }
}
