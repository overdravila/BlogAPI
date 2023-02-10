using AutoMapper;
using BlogAPI.Biz.Models;
using BlogAPI.Biz.Services.PostRejectionComments;
using BlogAPI.Contracts.Requests;
using BlogAPI.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [Route("api/postRejectionComment")]
    [ApiController]
    [Authorize]
    public class PostRejectionCommentController : ControllerBase
    {
        private readonly IPostRejectionCommentService _postRejectionCommentService;
        private readonly IMapper _mapper;

        public PostRejectionCommentController(IPostRejectionCommentService postRejectionCommentService, IMapper mapper)
        {
            _postRejectionCommentService = postRejectionCommentService;
            _mapper = mapper;
        }



        /// <summary>
        /// Create a post rejection comment
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Editor")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostRejectionComment))]
        public async Task<IActionResult> CreatePostRejectionComment([FromBody] PostRejectionCommentRequest request)
        {
            var postRejectionComment = _mapper.Map<PostRejectionComment>(request);
            var result = await _postRejectionCommentService.CreatePostRejectionComment(postRejectionComment);
            return Ok(result);
        }

        [HttpGet("post/{postId}")]
        [Authorize("EditorWriter")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostRejectionComment))]
        public async Task<IActionResult> GetPostRejectionCommentForPost([FromRoute] int postId)
        {
            var postRejectionComment = await _postRejectionCommentService.GetPostRejectionCommentByPostId(postId);
            if (postRejectionComment is null) return NotFound();
            return Ok(postRejectionComment);
        }
    }
}
