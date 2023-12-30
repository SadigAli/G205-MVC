using Ecommerce.Data.DTOs.ProductDTO;
using Ecommerce.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Contracts
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        public Task<List<Product>> GetAllProductsWithDetails();
        public Task<Product> GetProductWithDetails(int id);
        public void AddColors(Product product, List<int> colorIds);
        public Task Update(Product product, ProductPutDTO model);
        public Task AddImages(Product product, List<IFormFile> files, string wwwroot);
        public void RemoveImages(Product product,ImageStatus status);
        public Task<ProductColor> GetProductColor(int id);
        public Task AddSizes(ProductColor productColor,List<int> sizeIds);
        public Task<ProductColorSize> GetProductColorSize(int id);
        public Task UpdateProductColorSize(ProductColorSize productColorSize,ProductColorSizeDTO model);
        public Task<Product> GetProductBySlug(string slug);
        public List<Product> Filter(string search,string color,string size, string category, int? min, int? max);
    }
}
