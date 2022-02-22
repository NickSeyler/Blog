#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Blog.Services.Interfaces;
using Blog.Services;
using X.PagedList;

namespace Blog.Controllers
{
    public class BlogPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly SlugService _slugService;
        private readonly SearchService _searchService;

        public BlogPostsController(ApplicationDbContext context,
                                   IImageService imageService,
                                   SlugService slugService, 
                                   SearchService searchService)
        {
            _context = context;
            _imageService = imageService;
            _slugService = slugService;
            _searchService = searchService;
        }

        public async Task<IActionResult> BlogChildIndex(int blogItemId)
        {
            var children = await _context.BlogPosts.Include(b => b.BlogItem)
                                                   .Where(b => b.BlogItemId == blogItemId)
                                                   .ToListAsync();
            return View("Index", children);
        }

        [Authorize(Roles = "Administrator")]
        // GET: BlogPosts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BlogPosts.Include(b => b.BlogItem);
            return View(await applicationDbContext.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> SearchIndex(int? pageNum, string searchTerm)
        {
            pageNum ??= 1;
            var pageSize = 3;

            var posts = _searchService.TermSearch(searchTerm);
            var pagedPosts = await posts.ToPagedListAsync(pageNum, pageSize);

            ViewData["SearchTerm"] = searchTerm;
            return View(pagedPosts);
        }

        // GET: BlogPosts/Details/5
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts
                .Include(b => b.BlogItem)
                .Include(c => c.Comments)
                .ThenInclude(c => c.Author)
                .FirstOrDefaultAsync(m => m.Slug == slug);

            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // GET: BlogPosts/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["BlogItemId"] = new SelectList(_context.BlogItems, "Id", "BlogName");
            ViewData["TagIds"] = new MultiSelectList(_context.Tags, "Id", "Text");
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogItemId,Title,Abstract,BlogPostState,Body")] BlogPost blogPost, List<int> tagIds)
        {
            if (ModelState.IsValid)
            {
                var slug = _slugService.UrlFriendly(blogPost.Title);
                var isUnique = !_context.BlogPosts.Any(b => b.Slug == slug);
                if (isUnique)
                {
                    blogPost.Slug = slug;
                }
                else
                {
                    ModelState.AddModelError("Title", "This Title cannot be used (duplicate Slug).");
                    ViewData["BlogItemId"] = new SelectList(_context.BlogItems, "Id", "BlogName", blogPost.BlogItemId);
                    ViewData["TagIds"] = new MultiSelectList(_context.Tags, "Id", "Text", tagIds);
                    return View(blogPost);
                }

                if (tagIds.Count > 0)
                {
                    var tags = _context.Tags;
                    foreach (var tagId in tagIds)
                    {
                        blogPost.Tags.Add(await tags.FindAsync(tagId));
                    }
                }

                blogPost.Created = DateTime.UtcNow;


                _context.Add(blogPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogItemId"] = new SelectList(_context.BlogItems, "Id", "BlogName", blogPost.BlogItemId);
            ViewData["TagIds"] = new MultiSelectList(_context.Tags, "Id", "Text", tagIds);
            return View(blogPost);
        }

        // GET: BlogPosts/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts.Include("Tags")
                                                   .FirstOrDefaultAsync(b => b.Id == id);

            var tagIds = await blogPost.Tags.Select(b => b.Id).ToListAsync();
            if (blogPost == null)
            {
                return NotFound();
            }

            ViewData["BlogItemId"] = new SelectList(_context.BlogItems, "Id", "BlogName", blogPost.BlogItemId);
            ViewData["TagIds"] = new MultiSelectList(_context.Tags, "Id", "Text", tagIds);

            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogItemId,Title,Slug,IsDeleted,Abstract,BlogPostState,Body,Created")] BlogPost blogPost, List<int> tagIds)
        {
            if (id != blogPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var slug = _slugService.UrlFriendly(blogPost.Title);

                    var isUnique = !_context.BlogPosts.Any(b => b.Slug == slug);

                    if (isUnique || blogPost.Slug == slug)
                    {
                        blogPost.Slug = slug;
                    }
                    else
                    {
                        ModelState.AddModelError("Title", "This Title cannot be used (duplicate Slug).");
                        ViewData["BlogItemId"] = new SelectList(_context.BlogItems, "Id", "BlogName", blogPost.BlogItemId);
                        ViewData["TagIds"] = new MultiSelectList(_context.Tags, "Id", "Text", tagIds);
                        return View(blogPost);
                    }

                    blogPost.Created = DateTime.SpecifyKind(blogPost.Created, DateTimeKind.Utc);
                    blogPost.Updated = DateTime.UtcNow;

                    _context.Update(blogPost);
                    await _context.SaveChangesAsync();

                    var currentBlogPost = await _context.BlogPosts.Include("Tags")
                                                                  .FirstOrDefaultAsync(b => b.Id == blogPost.Id);

                    currentBlogPost.Tags.Clear();
                    if (tagIds.Count > 0)
                    {
                        var tags = _context.Tags;
                        foreach (var tagId in tagIds)
                        {
                            blogPost.Tags.Add(await tags.FindAsync(tagId));
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogPostExists(blogPost.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogItemId"] = new SelectList(_context.BlogItems, "Id", "BlogName", blogPost.BlogItemId);
            ViewData["TagIds"] = new MultiSelectList(_context.Tags, "Id", "Text", tagIds);
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts
                .Include(b => b.BlogItem)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(id);
            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogPostExists(int id)
        {
            return _context.BlogPosts.Any(e => e.Id == id);
        }
    }
}
