#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blog.Data;
using Blog.Models;
using Blog.Enum;

namespace Blog.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BlogPostsAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns the specified number of latest Posts
        /// </summary>
        /// <param name="num">integer count of records</param>
        /// <returns>
        /// Returns a list of Blog Posts
        /// </returns>
        [HttpGet("GetTopXPosts/{num:int}")]
        public async Task<ActionResult<IEnumerable<BlogPost>>> GetTopXPosts(int num)
        {
            var posts = await _context.BlogPosts
                                .Where(b => !b.IsDeleted && b.BlogPostState == BlogPostState.ProductionReady)
                                .OrderByDescending(b => b.Created)
                                .Take(num)
                                .ToListAsync();

            return posts;
        }
    }
}
