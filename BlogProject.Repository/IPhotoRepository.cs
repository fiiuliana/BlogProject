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
        public Task<Photo> InsertAsync(PhotoCreate photoCreate, int applicationUserId);
        public Task<Photo> GetAsync(int photoId);
        public Task<List<Photo>> GetAllBtUserIdAsync(int applicationUserId);
        public Task<int> DeleteAsync(int photoId);

    }
}
