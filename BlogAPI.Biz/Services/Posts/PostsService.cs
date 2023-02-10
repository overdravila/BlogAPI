using System.Globalization;
using BlogAPI.Biz.Models;
using BlogAPI.Data;
using BlogAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Biz.Services.Posts
{
    public class PostsService : IPostsService
    {
        private readonly BlogAPIDbContext _dbContext;

        public PostsService(BlogAPIDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PostResponse?> CreatePost(Post post)
        {
            if (post is null)
                throw new ArgumentNullException(nameof(post));
            post.PostStatusId = await _dbContext
                .PostStatus
                .Where(x => x.Description == "UnSubmitted")
                .Select(x => x.PostStatusId)
                .FirstOrDefaultAsync();

            _dbContext.Post.Add(post);
            var saved = await _dbContext.SaveChangesAsync() > 0;
            var addedPost = await GetPostById(post.PostId);
            if (!saved) return null;
            return new PostResponse
            {
                PostId = addedPost.PostId,
                Title = addedPost.Title,
                Content = addedPost.Content,
                DateOfPublishing = addedPost.DateOfPublishing,
                Author = $"{addedPost.Author.FirstName} {addedPost.Author.LastName}",
                PostStatus = addedPost.PostStatus.Description,
            };
        }

        public async Task<ICollection<PostResponse>> GetPostsByStatus(string statusDescription)
        {
            var posts = await _dbContext.Post
                .Include(x => x.Author)
                .Include(x => x.PostStatus)
                .Where(x => x.PostStatus.Description == statusDescription)
                .ToListAsync();
            var response = new List<PostResponse>();

            foreach (var post in posts)
            {
                response.Add(new PostResponse
                {
                    PostId = post.PostId,
                    Title = post.Title,
                    Content = post.Content,
                    DateOfPublishing = post.DateOfPublishing,
                    Author = $"{post.Author.FirstName} {post.Author.LastName}",
                    PostStatus = post.PostStatus.Description
                });
            }

            return response;
        }

        public async Task<ICollection<PostResponse>> GetPersonPosts(int personId)
        {
            var posts = await _dbContext.Post
                .Where(x => x.AuthorId == personId)
                .Include(x => x.Author)
                .Include(x => x.PostStatus)
                .ToListAsync();

            var response = new List<PostResponse>();

            foreach (var post in posts)
            {
                response.Add(new PostResponse
                {
                    PostId = post.PostId,
                    Title = post.Title,
                    Content = post.Content,
                    DateOfPublishing = post.DateOfPublishing,
                    Author = $"{post.Author.FirstName} {post.Author.LastName}",
                    PostStatus = post.PostStatus.Description
                });
            }

            return response;
        }

        public async Task<Post> GetPostById(int postId)
        {
            return await _dbContext.Post
                .Where(x => x.PostId == postId)
                .Include(x => x.Author)
                .Include(x => x.PostStatus)
                .FirstOrDefaultAsync();
        }

        public async Task<PostResponse?> GetPostResponseById(int postId)
        {
            var post = await GetPostById(postId);
            if (post is null) return null;
            return new PostResponse
            {
                PostId = post.PostId,
                Title = post.Title,
                Content = post.Content,
                DateOfPublishing = post.DateOfPublishing,
                Author = $"{post.Author.FirstName} {post.Author.LastName}",
                PostStatus = post.PostStatus.Description
            };
        }

        public async Task<PostResponse?> UpdatePost(Post post)
        {
            if (post is null)
                throw new ArgumentNullException(nameof(post));
            _dbContext.Post.Update(post);
            await _dbContext.SaveChangesAsync();
            var updatedPost = await GetPostById(post.PostId);
            return new PostResponse
            {
                PostId = updatedPost.PostId,
                Title = updatedPost.Title,
                Content = updatedPost.Content,
                DateOfPublishing = updatedPost.DateOfPublishing,
                Author = $"{updatedPost.Author.FirstName} {updatedPost.Author.LastName}",
                PostStatus = updatedPost.PostStatus.Description
            };
        }

        public async Task<PostResponse?> UpdatePostStatus(int postId, string postStatusDescription)
        {
            var post = await GetPostById(postId);
            if (post is null) return null;

            post.PostStatusId = await _dbContext
                .PostStatus
                .Where(x => x.Description == postStatusDescription)
                .Select(x => x.PostStatusId)
                .FirstOrDefaultAsync();

            _dbContext.Post.Update(post);
            await _dbContext.SaveChangesAsync();
            var updatedPost = await GetPostById(post.PostId);
            return new PostResponse
            {
                PostId = updatedPost.PostId,
                Title = updatedPost.Title,
                Content = updatedPost.Content,
                DateOfPublishing = updatedPost.DateOfPublishing,
                Author = $"{updatedPost.Author.FirstName} {updatedPost.Author.LastName}",
                PostStatus = updatedPost.PostStatus.Description
            };
        }
    }
}