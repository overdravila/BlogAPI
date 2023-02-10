using BlogAPI.Biz.Models;
using BlogAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Biz.Services.PostRejectionComments
{
    public interface IPostRejectionCommentService
    {
        Task<PostRejectionCommentResponse> CreatePostRejectionComment(PostRejectionComment postRejectionComment);
        Task<PostRejectionCommentResponse> GetPostRejectionCommentByPostId(int postId);
    }
}
