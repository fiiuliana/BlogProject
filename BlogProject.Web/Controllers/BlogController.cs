using BlogProject.Models.Blog;
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
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IPhotoRepository _photoRepository;

        //constructor
        public BlogController(IBlogRepository blogRepository, IPhotoRepository photoRepository)
        {
            _blogRepository = blogRepository;
            _photoRepository = photoRepository;
        }

        //ednpoints
        /// <summary>
        /// POST: creates a blog
        /// </summary>
        /// <param name="blogCreate">The blogCreate </param>
        /// <returns> the created blog</returns>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Blog>> Create(BlogCreate blogCreate)
        {
            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);
            
            // load the photo
            if (blogCreate.PhotoId.HasValue)
            {
                var photo = await _photoRepository.GetAsync(blogCreate.PhotoId.Value);

                //does the photo belong to the user?
                if (photo.ApplicationUserId != applicationUserId)
                {
                    return BadRequest("You did not upload the photo");
                }
            }
            var blog = await _blogRepository.UpsertAsync(blogCreate, applicationUserId);

            return Ok(blog);
        }

    
        /// <summary>
        /// GET: all the published blogs
        /// </summary>
        /// <param name="blogPaging"></param>
        /// <returns>All the blogs</returns>
        [HttpGet]
        public async Task<ActionResult<PagedResults<Blog>>> GetAll([FromQuery] BlogPaging blogPaging)
        {
            var blogs = await _blogRepository.getAllAsync(blogPaging);
            return Ok(blogs);
        }

        // the individual blog
        /// <summary>
        /// GET: a specific blog depending on the ID
        /// </summary>
        /// <param name="blogId">The Id of the blog</param>
        /// <returns> A specific blog</returns>
        [HttpGet("{blogId}")]
        public async Task<ActionResult<Blog>> Get(int blogId)
        {
            var blog = await _blogRepository.GetAsync(blogId);
            return Ok(blog);
        }

        // all the user's blogs
        /// <summary>
        /// GET: all the blogs that belong to a specific user
        /// </summary>
        /// <param name="applicationUserId"> The ID of the user</param>
        /// <returns>Blogs of the user</returns>
        [HttpGet("user/{applicationUserId}")]
        public async Task<ActionResult<List<Blog>>> GetByApplicationUserId(int applicationUserId)
        {
            var blogs = await _blogRepository.GetAllByUserIdAsync(applicationUserId);
            return Ok(blogs);
        }

        // get all the famous blogs
        /// <summary>
        /// GET: The firts 6 blogs based on number comments
        /// </summary>
        /// <returns>The first 6 blogs depending on the number of comments</returns>
        [HttpGet("famous")]
        public async Task<ActionResult<List<Blog>>> GetAllFamous()
        {
            var blogs = await _blogRepository.GetAllFamousAsync();

            return Ok(blogs);
        }

        /// <summary>
        /// DEL: deletes the desired blog 
        /// </summary>
        /// <param name="blogId"> The ID of the blog</param>
        /// <returns> Ok message if action is successful</returns>
        [Authorize]
        [HttpDelete("{blogId}")]
        public async Task<ActionResult<int>> Delete(int blogId)
        {
            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

            var foudBlog = await _blogRepository.GetAsync(blogId);

            if (foudBlog == null) return BadRequest("Blog does not exist");
            if (foudBlog.ApplicationUserId == applicationUserId)
            {
                var affectedRows = await _blogRepository.DeleteAsync(blogId);

                return Ok(affectedRows);
            }
            else {
                // return Unauthorized("You are not authorized");
                return BadRequest("The blog was not created by you");
            }
        }


    }
}
