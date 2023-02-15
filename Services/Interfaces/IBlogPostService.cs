using JABlog.Models;

namespace JABlog.Services.Interfaces
{
    public interface IBlogPostService
    {
        //CRUD SERVICES


        #region BlogPost CRUD METHODS
        public Task AddBlogPostAsync(BlogPost blogPost);

        public Task UpdateBlogPostAsync(BlogPost blogPost);

        public Task<BlogPost> GetBlogPostByIdAsync(int blogPostId);

        public Task DeleteBlogPostAsync(BlogPost blogPost);
        #endregion

        #region Get BlogPosts Methods
        public Task<IEnumerable<BlogPost>> GetAllBlogPostsAsync();
        public Task<IEnumerable<BlogPost>> GetPopularPostsAsync();
        public Task<IEnumerable<BlogPost>> GetPopularPostsAsync(int count);
        public Task<IEnumerable<BlogPost>> GetRecentPostsAsync();
        public Task<IEnumerable<BlogPost>> GetRecentPostsAsync(int count);
        #endregion

        #region Category CRUD
        public Task AddCategoryAsync(Category category);
        public Task UpdateCategoryAsync (Category category);
        public Task<Category> GetCategoryByIdAsync(int categoryId);
        public Task DeleteCategoryAsync(Category category);
        #endregion

        public Task<IEnumerable<Category>> GetAllCategoriesAsync();


        #region Tag CRUD
        public Task AddTagAsync(Tag tag);
        public Task UpdateTagAsync(Tag tag);
        public Task<Tag> GetTagByIdAsync(int tagId);
        public Task DeleteTagAsync(Tag tag);
        #endregion

    }
}
