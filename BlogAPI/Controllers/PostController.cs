using System.Net;
using System.Security.Claims;
using AutoMapper;
using BlogAPI.Biz.Models;
using BlogAPI.Biz.Services.People;
using BlogAPI.Biz.Services.Posts;
using BlogAPI.Contracts.Requests;
using BlogAPI.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [Route("api/post")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostsService _postsService;
        private readonly IMapper _mapper;
        private readonly IPeopleService _peopleService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostController(IPostsService postsService,
            IMapper mapper,
            IPeopleService peopleService,
            UserManager<ApplicationUser> userManager)
        {
            _postsService = postsService;
            _mapper = mapper;
            _peopleService = peopleService;
            _userManager = userManager;
        }

        /// <summary>
        /// Gets posts pending approval
        /// </summary>
        /// <returns></returns>
        [HttpGet("pending")]
        [Authorize(Policy = "Editor")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<PostResponse>))]
        public async Task<IActionResult> GetPendingPosts()
        {
            return Ok(await _postsService.GetPostsByStatus("Pending Approval"));
        }

        /// <summary>
        /// Gets published posts
        /// </summary>
        /// <returns></returns>
        [HttpGet("published")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<PostResponse>))]
        public async Task<IActionResult> GetPublishedPosts()
        {
            return Ok(await _postsService.GetPostsByStatus("Approved"));
        }

        /// <summary>
        /// Gets a post by its Id
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet("{postId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostResponse))]
        public async Task<IActionResult> GetPostById([FromRoute] int postId)
        {
            var response = await _postsService.GetPostResponseById(postId);
            if (response is null) return NotFound();
            return Ok(response);
        }

        /// <summary>
        /// Gets posts for logged user
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        [HttpGet("author")]
        [Authorize("Writer")]
        public async Task<IActionResult> GetPostsByAuthorId()
        {
            var person = await _peopleService.GetPersonByEmail(GetLoggedUserEmail());   
            return Ok(await _postsService.GetPersonPosts(person.PersonId));
        }

        /// <summary>
        /// Creates a post entry
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Writer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostResponse))]
        public async Task<IActionResult> CreatePost([FromBody] PostRequest request)
        {
            var post = _mapper.Map<Post>(request);
            var response = await _postsService.CreatePost(post);
            return Ok(response);
        }

        /// <summary>
        /// Updates a post
        /// </summary>
        /// <param name="request"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpPut("{postId}")]
        [Authorize(Policy = "Writer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostResponse))]
        public async Task<IActionResult> UpdatePost([FromBody] PostRequest request, [FromRoute] int postId)
        {
            var post = await _postsService.GetPostById(postId);

            if (post is null) return NotFound();
            var isUserAllowedToEdit = await IsUserPostAuthor(post);

            if (!isUserAllowedToEdit) return StatusCode(403, "Can't edit this post");

            if (post.PostStatus.Description == "Approved" || post.PostStatus.Description == "Pending Approval")
                return StatusCode(403, "Can't edit a Post with status Approved or Pending Approval");

            _mapper.Map(request, post);
            var response = await _postsService.UpdatePost(post);
            return Ok(response);
        }

        /// <summary>
        /// Approves a post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpPut("approve/{postId}")]
        [Authorize(Policy = "Editor")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostResponse))]
        public async Task<IActionResult> ApprovePost([FromRoute] int postId)
        {
            var response = await _postsService.UpdatePostStatus(postId, "Approve");
            if (response is null) return NotFound();
            return Ok(response);
        }

        /// <summary>
        /// Submits a post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpPut("submit/{postId}")]
        [Authorize(Policy = "Writer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostResponse))]
        public async Task<IActionResult> SubmitPost([FromRoute] int postId)
        {
            var response = await _postsService.UpdatePostStatus(postId, "Pending Approval");
            if (response is null) return NotFound();
            return Ok(response);
        }

        /// <summary>
        /// Submits a post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpPut("reject/{postId}")]
        [Authorize(Policy = "Editor")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostResponse))]
        public async Task<IActionResult> RejectPost([FromRoute] int postId)
        {
            var response = await _postsService.UpdatePostStatus(postId, "Rejected");
            if (response is null) return NotFound();
            return Ok(response);
        }

        private async Task<bool> IsUserPostAuthor(Post? post)
        {
            string userEmail = GetLoggedUserEmail();
            var person = await _peopleService.GetPersonByEmail(userEmail);

            return post.AuthorId == person.PersonId;
        }

        private string GetLoggedUserEmail()
        {
            ClaimsPrincipal currentUser = this.User;
            var userEmail = _userManager.GetUserId(currentUser);
            return userEmail;
        }
    }
}
