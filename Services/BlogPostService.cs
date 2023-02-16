using JABlog.Services.Interfaces;
using JABlog.Models;
using JABlog.Data;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace JABlog.Services
{
    public class BlogPostService : IBlogPostService
    {
        private readonly ApplicationDbContext _context;
        public BlogPostService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddBlogPostAsync(BlogPost blogPost)
        {
            try
            {
                _context.Add(blogPost);
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task AddCategoryAsync(Category category)
        {
            try
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task AddTagAsync(Tag tag)
        {
            try
            {
                _context.Add(tag);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteBlogPostAsync(BlogPost blogPost)
        {
            try
            {
                blogPost.IsDeleted = true;
                await UpdateBlogPostAsync(blogPost);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteCategoryAsync(Category category)
        {
            try
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteTagAsync(Tag tag)
        {
            try
            {
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            try
            {
                IEnumerable<Tag> tags = await _context.Tags.ToListAsync();
                return tags;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<BlogPost>> GetAllBlogPostsAsync()
        {
            try
            {
                IEnumerable<BlogPost> blogPosts = await _context.BlogPosts.Include(b => b.Category).Include(b => b.Tags).Include(b => b.Comments).ToListAsync();
                return blogPosts;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<BlogPost> GetBlogPostByIdAsync(int blogPostId)
        {
            try
            {
                BlogPost? blogPost = await _context.BlogPosts.Include(b => b.Category).Include(b => b.Tags).Include(b => b.Comments).FirstOrDefaultAsync(b => b.Id == blogPostId);
                return blogPost!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            try
            {
                Category? category = await _context.Categories.Include(c => c.BlogPosts).FirstOrDefaultAsync();
                return category!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            try
            {
                IEnumerable<Category> categories = await _context.Categories.Include(c => c.BlogPosts).ToListAsync();
                return categories;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<BlogPost>> GetPopularPostsAsync(int count)
        {
            try
            {
                IEnumerable<BlogPost> blogPosts = await _context.BlogPosts
           .Where(b => b.IsPublished == true && b.IsDeleted == false)
           .Include(b => b.Category)
           .Include(b => b.Tags)
           .Include(b => b.Comments)
           .ThenInclude(c => c.Author)
           .ToListAsync();
                return blogPosts.OrderByDescending(b => b.Comments.Count).Take(count);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<IEnumerable<BlogPost>> GetPopularPostsAsync()
        {
            try
            {
                IEnumerable<BlogPost> blogPosts = await _context.BlogPosts
           .Where(b => b.IsPublished == true && b.IsDeleted == false)
           .Include(b => b.Category)
           .Include(b => b.Tags)
           .Include(b => b.Comments)
           .ThenInclude(c => c.Author)
           .ToListAsync();
                return blogPosts.OrderByDescending(b => b.Comments.Count);
            }
            catch (Exception)
            {

                throw;
            }

        }


        public async Task<IEnumerable<BlogPost>> GetRecentPostsAsync()
        {
            try
            {
                IEnumerable<BlogPost> blogPosts = await _context.BlogPosts
                    .Where(b => b.IsPublished == true && b.IsDeleted == false)
                    .Include(b => b.Category)
                    .Include(b => b.Tags)
                    .Include(b => b.Comments)
                    .ThenInclude(c => c.Author)
                    .ToListAsync();
                return blogPosts.OrderByDescending(b => b.Created);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<BlogPost>> GetRecentPostsAsync(int count)
        {
            try
            {
                IEnumerable<BlogPost> blogPosts = await _context.BlogPosts
                    .Where(b => b.IsPublished == true && b.IsDeleted == false)
                    .Include(b => b.Category)
                    .Include(b => b.Tags)
                    .Include(b => b.Comments)
                    .ThenInclude(c => c.Author)
                    .ToListAsync();
                return blogPosts.OrderByDescending(b => b.Created).Take(count);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<Tag> GetTagByIdAsync(int tagId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateBlogPostAsync(BlogPost blogPost)
        {
            try
            {
                _context.Update(blogPost);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task UpdateCategoryAsync(Category category)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTagAsync(Tag tag)
        {
            throw new NotImplementedException();
        }
    }
}
