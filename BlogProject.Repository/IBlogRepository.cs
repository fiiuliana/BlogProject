using BlogProject.Models.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Repository
{
    public interface IBlogRepository
    {
        public Task<Blog> UpsertAsync(BlogCreate blogCreate, int applicationUserId);
        public Task<PagedResults<Blog>> getAllAsync(BlogPaging blogPaging);

        public Task<Blog> GetAsync(int blogId);
        public Task<List<Blog>> GetAllByUserIdAsync(int applicationUserId);
        public Task<List<Blog>> GetAllFamousAsync();
        public Task<int> DeleteAsync(int blogId);


    }
}
