using BlogProject.Models.Photo;
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
       
    public class PhotoRepository : IPhotoRepository
    {
        private readonly IConfiguration _config;

        public PhotoRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<int> DeleteAsync(int photoId)
        {
            int affectedRows = 0;

            
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {  
                //get the default connection
                await connection.OpenAsync();

                affectedRows = await connection.ExecuteAsync(  //get back number of rows which are affected
                    "Photo_Delete",
                    new { PhotoId = photoId },
                    commandType: CommandType.StoredProcedure);   // procedure in SQL DB
            }
            return affectedRows;
        }

        // Will get back the list of photos
        public async Task<List<Photo>> GetAllByUserIdAsync(int applicationUserId)
        {
            // an emptyphoto list
            IEnumerable<Photo> photos;

           
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                // create a connection
                await connection.OpenAsync();

                photos = await connection.QueryAsync<Photo>(  // returns a type of pthoto
                    "Photo_GetByUserId",
                    new { ApplicationUserId = applicationUserId },
                    commandType: CommandType.StoredProcedure   // procedure in SQL DB
                    );
            }

            return photos.ToList();
        }


        public async Task<Photo> GetAsync(int photoId)
        {
            Photo photo;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                // open connection
                await connection.OpenAsync();

                photo = await connection.QueryFirstOrDefaultAsync<Photo>(  // return a type of photo
                    "Photo_Get",
                    new { PhotoId = photoId },
                    commandType: CommandType.StoredProcedure // procedure in SQL DB
                    );
            }
            return photo;
        }


        public async Task<Photo> InsertAsync(PhotoCreate photoCreate, int applicationUserId)
        {

            // create a new data table 
            var dataTable = new DataTable();
            dataTable.Columns.Add("PublicId", typeof(string));
            dataTable.Columns.Add("ImageUrl", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));

            dataTable.Rows.Add(photoCreate.PublicId, photoCreate.ImageUrl, photoCreate.Description);

            int newPhotoId;
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                // open a connection
                await connection.OpenAsync();

                newPhotoId = await connection.ExecuteScalarAsync<int>( // getting a number
                    "Photo_Insert",
                    new { 
                        Photo = dataTable.AsTableValuedParameter("dbo.PhotoType"),
                        ApplicationUserId = applicationUserId
                    },
                    commandType: CommandType.StoredProcedure  // procedure in SQL DB
                    );
            }
            //new insterted photo will be loaded and returned
            Photo photo = await GetAsync(newPhotoId);
            return photo;
        }
    }
}
