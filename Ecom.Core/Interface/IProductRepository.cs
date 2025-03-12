using Ecom.Core.Entities.Product;
using Ecom.Core.Models.Product;
using Ecom.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Interface
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<bool> AddAsync(AddProductDto addProductDto);
        Task<bool> UpdateAsync(UpdateProductDto updateProductDto);
        Task<bool> DeleteAsync(Product product);
        Task<ReturnProductDto> GetAllAsync(ProductParams productParams);

    }
}
