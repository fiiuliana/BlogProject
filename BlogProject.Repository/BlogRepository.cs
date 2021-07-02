using BlogProject.Models.Blog;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Repository
{
    public class BlogRepository : IBlogRepository
    {

        private readonly IConfiguration _config;

        public BlogRepository(IConfiguration config)
        {
            _config = config;
        }


        public async Task<int> DeleteAsync(int blogId)
        {
            int affectedRows = 0;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            { 
                //open a connection
                await connection.OpenAsync();

                affectedRows = await connection.ExecuteAsync(
                    "Blog_Delete",
                    new { BlogId = blogId },
                    commandType: CommandType.StoredProcedure); // procedure in SQL DB
            }
            return affectedRows;
        }


        public async Task<PagedResults<Blog>> getAllAsync(BlogPaging blogPaging)
        {
            var results = new PagedResults<Blog>();

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                // open a connection
                await connection.OpenAsync();

                using (var multi = await connection.QueryMultipleAsync( // getting multiple results from the stored procedure
                    "Blog_GetAll",
                    new
                    { 
                        Offset = (blogPaging.Page - 1) * blogPaging.Pagesize,  // calculate the offset and pass to SQL
                        PageSize = blogPaging.Pagesize
                    },
                    commandType: CommandType.StoredProcedure)) // procedure in SQL DB
                {
                    results.Items = multi.Read<Blog>();     //getting the results from the query
                    results.TotalCount = multi.ReadFirst<int>();
                }
            }
            return results;
        }

        public async Task<List<Blog>> GetAllByUserIdAsync(int applicationUserId)
        {
            IEnumerable<Blog> blogs; 

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            { 
                // open a connection
                await connection.OpenAsync();

                blogs = await connection.QueryAsync<Blog>(
                    "Blog_GetByUserId",
                    new { ApplicationUserId = applicationUserId },
                    commandType: CommandType.StoredProcedure              // procedure in SQL DB
                    );
            }
            return blogs.ToList();
        }

        public async Task<List<Blog>> GetAllFamousAsync()
        {
            IEnumerable<Blog> famousBlogs;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                // open a connection
                await connection.OpenAsync();

                famousBlogs = await connection.QueryAsync<Blog>(
                    "Blog_GetAllFamous",
                    //initialize an empty list
                    new {  },
                    commandType: CommandType.StoredProcedure   // procedure in SQL DB
                    );
            }
            return famousBlogs.ToList();
        }


        public async Task<Blog> GetAsync(int blogId)
        {
            Blog blog;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                //open a connection
                await connection.OpenAsync();

                blog = await connection.QueryFirstOrDefaultAsync<Blog>(
                    "Blog_Get",
                    new { BlogId = blogId }, // passing the parameter for the stored procedure
                    commandType: CommandType.StoredProcedure   // procedure in SQL DB
                    );
            }
            return blog;
        }

        public async Task<Blog> UpsertAsync(BlogCreate blogCreate, int applicationUserId)
        {
            // new data table
            var dataTable = new DataTable();
            dataTable.Columns.Add("BlogId", typeof(int));
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("Content", typeof(string));
            dataTable.Columns.Add("PhotoId", typeof(int));

            dataTable.Rows.Add(blogCreate.blogId, blogCreate.Title, blogCreate.Content, blogCreate.PhotoId);

            int? newBlogId;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                //open a connection and grab the above
                await connection.OpenAsync();

                newBlogId = await connection.ExecuteScalarAsync<int?>(
                    "Blog_Upsert",
                    new
                    {
                        Blog = dataTable.AsTableValuedParameter("dbo.BlogType"),  // pass in the object
                        ApplicationUserId = applicationUserId                     
                    },
                    commandType: CommandType.StoredProcedure  // procedure in SQL DB
                    );
            }

            // the update will not return a new one, but the initial
            newBlogId = newBlogId ?? blogCreate.blogId;
            Blog blog = await GetAsync(newBlogId.Value);  // pass the new values
            return blog;
        }
    }
}
