using AutoMapper;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interface;
using Ecom.Core.Models.Product;
using Ecom.Infrastructure.Data;
using Ecom.Infrastructure.Repositories.Services;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        private readonly ImageManagementService _imageManagementService;

        public ProductRepository(AppDbContext appDbContext, IMapper mapper, ImageManagementService imageManagementService) : base(appDbContext)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
            _imageManagementService = imageManagementService;
        }

        public async Task<bool> AddAsync(AddProductDto addProductDto)
        {
            if (addProductDto == null)
                return false;
            var product = _mapper.Map<Product>(addProductDto);
            await _appDbContext.Products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();

            var imagePath = await _imageManagementService.AddImageAsync(addProductDto.Photo, addProductDto.Name);
            var photos = imagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = product.Id

            }).ToList();
            await _appDbContext.Photos.AddRangeAsync(photos);
            await _appDbContext.SaveChangesAsync();
            return true;

        }

        public async Task<bool> DeleteAsync(Product product)
        {
            var photos = await _appDbContext.Photos.Where(a => a.ProductId == product.Id).ToListAsync();
            foreach (var photo in photos)
            {
                await _imageManagementService.DeleteImageAsycn(photo.ImageName);
            }
            _appDbContext.Products.Remove(product);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(UpdateProductDto updateProductDto)
        {
            if (updateProductDto is null)
            {
                return false;
            }
            var findProduct = await _appDbContext.Products
                .Include(m => m.Category)
                .Include(m => m.Photos).FirstOrDefaultAsync(a => a.Id == updateProductDto.Id);
            if (findProduct is null)
            {
                return false;
            }

            _mapper.Map<UpdateProductDto>(findProduct);
            var findPhotos = await _appDbContext.Photos.Where(m => m.ProductId == updateProductDto.Id).ToListAsync();
            foreach (var photoItem in findPhotos)
            {
                await _imageManagementService.DeleteImageAsycn(photoItem.ImageName);
            }
            _appDbContext.Photos.RemoveRange(findPhotos);
            var imagePaht = await _imageManagementService.AddImageAsync(updateProductDto.Photo, updateProductDto.Name);

            var photos = imagePaht.Select(path => new Photo
            {
                ImageName = path,
                ProductId = updateProductDto.Id,
            }).ToList();

            await _appDbContext.Photos.AddRangeAsync(photos);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
