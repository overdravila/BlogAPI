using BlogAPI.Biz.Models;
using BlogAPI.Data;
using BlogAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Biz.Services.PostRejectionComments
{
    public class PostRejectionCommentService : IPostRejectionCommentService
    {
        private readonly BlogAPIDbContext _dbContext;

        public PostRejectionCommentService(BlogAPIDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PostRejectionCommentResponse> CreatePostRejectionComment(PostRejectionComment postRejectionComment)
        {
            _dbContext.PostRejectionComment.Add(postRejectionComment);
            var saved = await _dbContext.SaveChangesAsync() > 0;
            if (!saved) return null;

            var addedPostRejectionComment = await _dbContext.PostRejectionComment
                .Where(x => x.PostRejectionCommentId == postRejectionComment.PostRejectionCommentId)
                .Include(x => x.Author)
                .Include(x => x.Post)
                .FirstOrDefaultAsync();

            return new PostRejectionCommentResponse
            {
                PostRejectionCommentId = addedPostRejectionComment.PostRejectionCommentId,
                Description = addedPostRejectionComment.Description,
                Author = $"{addedPostRejectionComment.Author.FirstName} {addedPostRejectionComment.Author.LastName}",
                PostTitle = addedPostRejectionComment.Post.Title
            };
        }

        public async Task<PostRejectionCommentResponse> GetPostRejectionCommentByPostId(int postId)
        {
            var postRejectionComment = await _dbContext.PostRejectionComment
                .Where(x => x.PostId == postId)
                .Include(x => x.Author)
                .Include(x => x.Post)
                .FirstOrDefaultAsync();

            return new PostRejectionCommentResponse
            {
                PostRejectionCommentId = postRejectionComment.PostRejectionCommentId,
                Description = postRejectionComment.Description,
                Author = $"{postRejectionComment.Author.FirstName} {postRejectionComment.Author.LastName}",
                PostTitle = postRejectionComment.Post.Title
            };
        }
    }
}
