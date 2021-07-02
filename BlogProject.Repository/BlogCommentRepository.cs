using BlogProject.Models.BlogComment;
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
    public class BlogCommentRepository : IBlogCommentRepository
    {

        private readonly IConfiguration _config;

        public BlogCommentRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<int> DeleteAsync(int blogCommentId)
        {
            int affectedRows = 0;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {  
                //open a connection
                await connection.OpenAsync();

                affectedRows = await connection.ExecuteAsync( // from dapper
                    "BlogComment_Delete",
                    new { BlogCommentId = blogCommentId }, // pass the id
                    commandType: CommandType.StoredProcedure);  // procedure in SQL DB
            }
            return affectedRows;
        }

        public async Task<List<BlogComment>> GetAllAsync(int blogId)
        {
            IEnumerable<BlogComment> blogComments;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {

                //open the connection
                await connection.OpenAsync();

                blogComments = await connection.QueryAsync<BlogComment>(
                    "BlogComment_GetAll",
                    new { BlogId = blogId },
                    commandType: CommandType.StoredProcedure // procedure in SQL DB
                    );
            }
            return blogComments.ToList();
        }

        public async Task<BlogComment> GetAsync(int blogCommentId)
        {
            BlogComment blogComment;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            { 
                //open a connection
                await connection.OpenAsync();

                blogComment = await connection.QueryFirstOrDefaultAsync<BlogComment>(
                    "BlogComment_Get",
                    new { BlogCommentId = blogCommentId },  // pass the parameter
                    commandType: CommandType.StoredProcedure // procedure in SQL DB
                    );
            }
            return blogComment;
        }

        public async Task<BlogComment> UpsertAsync(BlogCommentCreate blogCommentCreate, int applicationUserId)
        {
            //create a new table
            var dataTable = new DataTable();
            dataTable.Columns.Add("BlogCommentId", typeof(int));
            dataTable.Columns.Add("ParentBlogCommentId", typeof(int));
            dataTable.Columns.Add("BlogId", typeof(int));
            dataTable.Columns.Add("Content", typeof(string));

            dataTable.Rows.Add(
                blogCommentCreate.BlogCommentId, 
                blogCommentCreate.ParentBlogCommentId, 
                blogCommentCreate.BlogId, 
                blogCommentCreate.Content);

            int? newBlogCommentId;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
               // open a connection     
                await connection.OpenAsync();

                newBlogCommentId = await connection.ExecuteScalarAsync<int?>(
                    "BlogComment_Upsert",
                    new { BlogComment = dataTable.AsTableValuedParameter("dbo.BlogCommentType"), // pass the object
                    ApplicationUserId = applicationUserId
                    },
                    commandType: CommandType.StoredProcedure   // procedure in SQL DB
                    );
            }

            // 
            newBlogCommentId = newBlogCommentId ?? blogCommentCreate.BlogCommentId;

            BlogComment blogComment = await GetAsync(newBlogCommentId.Value);

            return blogComment;
        }
    }
}
