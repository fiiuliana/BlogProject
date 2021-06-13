using BlogProject.Models.Photo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Repository
{
    public class PhotoRepository : IPhotoRepository
    {
        public Task<Photo> DeleteAsynk(int photoId)
        {
            throw new NotImplementedException();
        }

        public Task<Photo> GetAllBtUserIdAsynk(int applicationUserId)
        {
            throw new NotImplementedException();
        }

        public Task<Photo> GetAsynk(int photoId)
        {
            throw new NotImplementedException();
        }

        public Task<Photo> InsertAsynk(PhotoCreate photoCreate, int applicationUserId)
        {
            throw new NotImplementedException();
        }
    }
}
