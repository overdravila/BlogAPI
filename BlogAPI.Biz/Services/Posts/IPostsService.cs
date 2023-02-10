using BlogAPI.Biz.Models;
using BlogAPI.Data.Models;

namespace BlogAPI.Biz.Services.Posts
{
    public interface IPostsService
    {
        Task<PostResponse?> GetPostResponseById(int postId);
        Task<Post> GetPostById(int postId);
        Task<ICollection<PostResponse>> GetPersonPosts(int personId);
        Task<PostResponse?> CreatePost(Post post);
        Task<PostResponse?> UpdatePost(Post post);
        Task<PostResponse?> UpdatePostStatus(int postId, string postStatusDescription);
        Task<ICollection<PostResponse>> GetPostsByStatus(string statusDescription);
    }
}