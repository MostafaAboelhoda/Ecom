using Ecom.Core.Entities.Product;
using Ecom.Core.Interface;
using Ecom.Infrastructure.Data;

namespace Ecom.Infrastructure.Repositories
{
    public class PhotoRepository : GenericRepository<Photo>, IPhotoRepository
    {
        public PhotoRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
