using AutoMapper;
using BlogAPI.Biz.Models;
using BlogAPI.Biz.Services.Comments;
using BlogAPI.Biz.Services.Posts;
using BlogAPI.Contracts.Requests;
using BlogAPI.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [Route("api/comment")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentsService _commentsService;
        private readonly IMapper _mapper;

        
        public CommentController(ICommentsService commentsService, IMapper mapper)
        {
            _commentsService = commentsService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a comment by its Id
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpGet("{commentId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentResponse))]
        public async Task<IActionResult> GetCommentById([FromRoute] int commentId)
        {
            var comment = await _commentsService.GetCommentById(commentId);
            if (comment is null) return NotFound();

            return Ok(comment);
        }

        /// <summary>
        /// Gets all comments for a post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet("post/{postId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<CommentResponse>))]
        public async Task<IActionResult> GetCommentsForPost([FromRoute] int postId)
        {
            return Ok(await _commentsService.GetCommentsForPost(postId));
        }

        /// <summary>
        /// Creates a comment
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentResponse))]
        public async Task<IActionResult> CreateComment([FromBody] CommentRequest request)
        {
            var comment = _mapper.Map<Comment>(request);
            var response = await _commentsService.CreateComment(comment);
            if (response is null) return StatusCode(403, "Can't add comments to this post");
            return Ok(response);

        }
    }
}