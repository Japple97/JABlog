using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JABlog.Data;
using JABlog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using JABlog.Services.Interfaces;
using System.Runtime.InteropServices;

namespace JABlog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BlogPostsController : Controller
    {

        private readonly UserManager<BlogUser> _userManager;
        private readonly IImageService _imageService;
        private readonly IBlogPostService _blogPostService;

        public BlogPostsController(ApplicationDbContext context,
                                   UserManager<BlogUser> userManager,
                                   IImageService imageService,
                                   IBlogPostService blogPostService)
        {           
            _userManager = userManager;
            _imageService = imageService;
            _blogPostService = blogPostService;
        }

        // GET: BlogPosts
        public async Task<IActionResult> AdminPage()
        {

            var blogPosts = await _blogPostService.GetAllBlogPostsAsync();
            return View(blogPosts);
        }

        // GET: BlogPosts/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _blogPostService.GetBlogPostByIdAsync(id.Value);

            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // GET: BlogPosts/Create
        public async Task<IActionResult> Create()
        {

            ViewData["Tags"] = new MultiSelectList(await _blogPostService.GetAllTagsAsync(), "Id", "Name");
            ViewData["CategoryId"] = new SelectList(await _blogPostService.GetAllCategoriesAsync(), "Id", "Name");
            return View(new BlogPost());
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Abstract,Content,Created,Updated,Slug,IsDeleted,IsPublished,ImageFile,CategoryId")] BlogPost blogPost)
        {
            
            if (ModelState.IsValid)
            {
                //TODO: Slug Blogpost

                //Format Dates
                blogPost.Updated = DateTime.UtcNow;
                blogPost.Created = DateTime.UtcNow;

                if (blogPost.ImageFile != null)
                {
                    blogPost.ImageData = await _imageService.ConvertFileToByteArrayAsync(blogPost.ImageFile);
                    blogPost.ImageType = blogPost.ImageFile.ContentType;
                }

                await _blogPostService.AddBlogPostAsync(blogPost);
                return RedirectToAction(nameof(Index));
            }


            ViewData["Tags"] = new MultiSelectList(await _blogPostService.GetAllTagsAsync(), "Id", "Name", blogPost.Tags);
            ViewData["CategoryId"] = new SelectList(await _blogPostService.GetAllCategoriesAsync(), "Id", "Name", blogPost.CategoryId);
            return View(blogPost);
        }

        // GET: BlogPosts/Edit/5   
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _blogPostService.GetBlogPostByIdAsync((int)id);
            if (blogPost == null)
            {
                return NotFound();
            }



            ViewData["Tags"] = new MultiSelectList(await _blogPostService.GetAllTagsAsync(), "Id", "Name", blogPost.Tags);
            ViewData["CategoryId"] = new SelectList(await _blogPostService.GetAllCategoriesAsync(), "Id", "Name", blogPost.CategoryId);
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Abstract,Content,Created,Updated,Slug,IsDeleted,IsPublished,ImageFile,CategoryId")] BlogPost blogPost)
        {
            if (id != blogPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    blogPost.Created = DataUtility.GetPostGresDate(blogPost.Created);
                    blogPost.Updated = DataUtility.GetPostGresDate(DateTime.UtcNow);
                    if (blogPost.ImageFile != null)
                    {
                        blogPost.ImageData = await _imageService.ConvertFileToByteArrayAsync(blogPost.ImageFile);
                        blogPost.ImageType = blogPost.ImageFile.ContentType;
                    }
                    await _blogPostService.UpdateBlogPostAsync(blogPost);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await BlogPostExists(blogPost.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AdminPage));
            }
            ViewData["Tags"] = new MultiSelectList(await _blogPostService.GetAllTagsAsync(), "Id", "Name", blogPost.Tags);
            ViewData["CategoryId"] = new SelectList(await _blogPostService.GetAllCategoriesAsync(), "Id", "Name", blogPost.CategoryId);
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _blogPostService.GetBlogPostByIdAsync((int)id);
            
            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BlogPosts'  is null.");
            }
            var blogPost = await _blogPostService.GetBlogPostByIdAsync(id);

            if (blogPost != null)
            {
              await _blogPostService.DeleteBlogPostAsync(blogPost);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> BlogPostExists(int id)
        {
          return (await _blogPostService.GetAllBlogPostsAsync()).Any(e => e.Id == id);
        }
    }
}
