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
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Blog.Services;

namespace Blog.Controllers
{
    public class BlogItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;

        public BlogItemsController(ApplicationDbContext context,
                                   IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        // GET: BlogItems
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.BlogItems.ToListAsync());
        }

        // GET: BlogItems/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: BlogItems/Create/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogName,Description")] BlogItem blogItem, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    blogItem.ImageData = await _imageService.ConvertFileToByteArrayAsync(imageFile);
                    blogItem.ImageType = imageFile.ContentType;
                }

                blogItem.Created = DateTime.UtcNow;

                _context.Add(blogItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blogItem);
        }

        // GET: BlogItems/Edit
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogItem = await _context.BlogItems.FindAsync(id);
            if (blogItem == null)
            {
                return NotFound();
            }
            return View(blogItem);
        }

        // POST: BlogItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogName,Description,Created")] BlogItem blogItem, IFormFile imageFile)
        {
            if (id != blogItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null)
                    {
                        blogItem.ImageData = await _imageService.ConvertFileToByteArrayAsync(imageFile);
                        blogItem.ImageType = imageFile.ContentType;
                    }

                    blogItem.Created = DateTime.SpecifyKind(blogItem.Created, DateTimeKind.Utc);
                    blogItem.Updated = DateTime.UtcNow;

                    _context.Update(blogItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogItemExists(blogItem.Id))
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
            return View(blogItem);
        }

        // GET: BlogItems/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogItem = await _context.BlogItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogItem == null)
            {
                return NotFound();
            }

            return View(blogItem);
        }

        // POST: BlogItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogItem = await _context.BlogItems.FindAsync(id);
            _context.BlogItems.Remove(blogItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogItemExists(int id)
        {
            return _context.BlogItems.Any(e => e.Id == id);
        }
    }
}
