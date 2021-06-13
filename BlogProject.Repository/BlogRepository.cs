using BlogProject.Models.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Repository
{
    public class BlogRepository : IBlogRepository
    {
        public Task<int> DeleteAsync(int blogId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResults<Blog>> getAllAsync(BlogPaging blogPaging)
        {
            throw new NotImplementedException();
        }

        public Task<List<Blog>> GetAllByUserIdAsync(int applicationUserId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Blog>> GetAllFamousAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Blog> GetAsync(int blogId)
        {
            throw new NotImplementedException();
        }

        public Task<Blog> UpsertAsync(BlogCreate blogCreate, int applicationUserId)
        {
            throw new NotImplementedException();
        }
    }
}
