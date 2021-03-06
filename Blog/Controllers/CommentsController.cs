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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BlogUser> _userManager;

        public CommentsController(ApplicationDbContext context, 
                                  UserManager<BlogUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogPostId,CommentBody")] Comment comment, string slug)
        {
            if (ModelState.IsValid)
            {
                comment.AuthorId = _userManager.GetUserId(User);
                comment.CreatedDate = DateTime.UtcNow;

                _context.Add(comment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", "BlogPosts", new { slug },"CommentBox");
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CommentBody")] Comment comment, string slug)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            try
            {
                var commentSnapshot = await _context.Comment.FindAsync(comment.Id);

                if (commentSnapshot == null)
                {
                    return NotFound();
                }

                commentSnapshot.CommentBody = comment.CommentBody;
                commentSnapshot.UpdatedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(comment.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Details", "BlogPosts", new { slug }, "CommentSection");

        }

        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Moderate(int id, [Bind("Id", "ModeratedBody", "ModerationReason")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            var commentSnapshot = await _context.Comment.Include(c => c.BlogPost)
                                                        .FirstOrDefaultAsync(c => c.Id == comment.Id);

            try
            {
                if (commentSnapshot == null)
                {
                    return NotFound();
                }
                commentSnapshot.ModeratorId = _userManager.GetUserId(User);
                commentSnapshot.ModeratedBody = comment.ModeratedBody;
                commentSnapshot.ModerationReason = comment.ModerationReason;
                commentSnapshot.ModeratedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(comment.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Details", "BlogPosts", new { slug = commentSnapshot.BlogPost.Slug }, "CommentSection");
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comment.Include(c => c.BlogPost)
                                                .FirstOrDefaultAsync(c=> c.Id == id);
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "BlogPosts", new { slug = comment.BlogPost.Slug}, "CommentSection");
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.Id == id);
        }
    }
}
