using BlogAPI.Biz.Models;
using BlogAPI.Data.Models;

namespace BlogAPI.Biz.Services.Comments
{
    public interface ICommentsService
    {
        Task<CommentResponse> GetCommentById(int commentId);
        Task<ICollection<CommentResponse>> GetCommentsForPost(int postId);
        Task<CommentResponse> CreateComment(Comment comment);
    }
}