using AutoMapper;
using Ecom.Core.Interface;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using Ecom.Infrastructure.Repositories.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly ImageManagementService _imageManagementService;


        public ICategoryRepository CategoryRepository { get; }

        public IProductRepository ProductRepository { get; }

        public IPhotoRepository PhotoRepository { get; }
        public UnitOfWork(AppDbContext appDbContext, IMapper mapper, ImageManagementService imageManagementService)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _imageManagementService = imageManagementService;
            CategoryRepository = new CategoryRepository(_appDbContext);
            ProductRepository = new ProductRepository(_appDbContext, _mapper, _imageManagementService);
            PhotoRepository = new PhotoRepository(_appDbContext);
        }
    }
}
