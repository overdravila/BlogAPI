using BlogAPI.Biz.Models;
using BlogAPI.Data;
using BlogAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Biz.Services.Comments
{
    public class CommentsService : ICommentsService
    {
        private readonly BlogAPIDbContext _dbContext;

        public CommentsService(BlogAPIDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CommentResponse> CreateComment(Comment comment)
        {
            if (comment is null)
                throw new ArgumentNullException(nameof(comment));

            var post = await _dbContext.Post
                .Include(x => x.PostStatus)
                .Where(x => x.PostId == comment.PostId)
                .FirstOrDefaultAsync();

            if (post.PostStatus.Description != "Approved")
                return null;

            _dbContext.Comment.Add(comment);
            var saved = await _dbContext.SaveChangesAsync() > 0;
            if (!saved) return null;

            var addedComment = await _dbContext.Comment
                .Where(x => x.CommentId == comment.CommentId)
                .Include(x => x.Author)
                .FirstOrDefaultAsync();

            return new CommentResponse
            {
                CommentId = addedComment.CommentId,
                Content = addedComment.Content,
                Date = addedComment.Date,
                PostId = comment.PostId,
                Author = $"{addedComment.Author.FirstName} {addedComment.Author.LastName}"
            };
        }

        public async Task<CommentResponse> GetCommentById(int commentId)
        {
            var comment = await _dbContext.Comment
                .Where(x => x.CommentId == commentId)
                .Include(x => x.Author)
                .FirstOrDefaultAsync();

            return new CommentResponse
            {
                CommentId = comment.CommentId,
                Content = comment.Content,
                Date = comment.Date,
                PostId = comment.PostId,
                Author = $"{comment.Author.FirstName} {comment.Author.LastName}"
            };
        }

        public async Task<ICollection<CommentResponse>> GetCommentsForPost(int postId)
        {
            var comments = await _dbContext.Comment
                .Where(x => x.PostId == postId)
                .Include(x => x.Author)
                .ToListAsync();

            var commentResponses = new List<CommentResponse>();

            foreach (var comment in comments)
            {
                commentResponses.Add(new CommentResponse
                {
                    CommentId = comment.CommentId,
                    Content = comment.Content,
                    Date = comment.Date,
                    PostId = comment.PostId,
                    Author = $"{comment.Author.FirstName} {comment.Author.LastName}"
                });
            }

            return commentResponses;
        }
    }
}