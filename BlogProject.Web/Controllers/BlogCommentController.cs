using BlogProject.Models.BlogComment;
using BlogProject.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogCommentController : ControllerBase
    {
        // add the dependencies
        private readonly IBlogCommentRepository _blogCommentRepository;

        public BlogCommentController(IBlogCommentRepository blogCommentRepository)
        {
            _blogCommentRepository = blogCommentRepository;
        }

        /// <summary>
        /// POST: creates a new comment
        /// </summary>
        /// <param name="blogCommentCreate"> The created blog</param>
        /// <returns>The comment</returns>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BlogComment>> Create(BlogCommentCreate blogCommentCreate)
        {
            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

            var createdBlogComment = await _blogCommentRepository.UpsertAsync(blogCommentCreate, applicationUserId);

            return Ok(createdBlogComment);
        }
        /// <summary>
        /// GET: all the blogs based on the ID
        /// </summary>
        /// <param name="blogId"> The Id of the blog</param>
        /// <returns>The comment </returns>
        [HttpGet("{blogId}")]
        public async Task<ActionResult<List<BlogComment>>> GetAll(int blogId)
        {
            var blogComments = await _blogCommentRepository.GetAllAsync(blogId);

            return Ok(blogComments);
        }

        /// <summary>
        /// DEL: delete an existing comment
        /// </summary>
        /// <param name="blogCommentId">The Id of the blog</param>
        /// <returns>Ok if the action is successfull. ID of the deleted blog</returns>
        [Authorize]
        [HttpDelete("{blogCommentId}")]
        public async Task<ActionResult<int>> Delete(int blogCommentId)
        {
            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

            var foundBlogComment = await _blogCommentRepository.GetAsync(blogCommentId);

            if (foundBlogComment == null) return BadRequest ("Comment does not exist");

            if (foundBlogComment.ApplicationUserId == applicationUserId)
            {
                var affectedRows = await _blogCommentRepository.DeleteAsync(blogCommentId);
                return Ok(affectedRows);
            }
            else {
                return BadRequest("This comment was not created by you");
            }
        }
    }
}
