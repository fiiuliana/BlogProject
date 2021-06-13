using BlogProject.Models.Photo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Repository
{

    public interface IPhotoRepository
    {
        public Task<Photo> InsertAsynk(PhotoCreate photoCreate, int applicationUserId);
        public Task<Photo> GetAsynk(int photoId);
        public Task<List<Photo>> GetAllBtUserIdAsynk(int applicationUserId);
        public Task<int> DeleteAsynk(int photoId);

    }
}
