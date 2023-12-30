using Ecommerce.Data.DTOs.ProductDTO;
using Ecommerce.Data.Entities;
using Ecommerce.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Implementations
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly IFileService _fileService;
        public ProductRepository(ApplicationContext context, IFileService fileService) : base(context)
        {
            _fileService = fileService;
        }

        public void AddColors(Product product, List<int> colorIds)
        {
            
            foreach (int colorId in colorIds)
            {
                product.ProductColors.Add(new ProductColor { ColorId = colorId });
            }
        }

        public async Task AddImages(Product product, List<IFormFile> files,string wwwroot)
        {
            foreach (IFormFile file in files)
            {

                (bool status, string message) = await _fileService.FileUpload(file, wwwroot, "products");
                if (status)
                {
                    product.ProductImages.Add(new ProductImage
                    {
                        Status = ImageStatus.Normal,
                        Image = message
                    });
                }
            }
        }

        public async Task AddSizes(ProductColor productColor, List<int> sizeIds)
        {
            productColor.ProductColorSizes = new List<ProductColorSize>();
            foreach (int sizeId in sizeIds)
            {
                productColor.ProductColorSizes.Add(new ProductColorSize
                {
                    SizeId = sizeId,
                    Count = 0
                });
            }
            await _context.SaveChangesAsync();
        }

        public List<Product> Filter(string search, string color, string size, string category, int? min, int? max)
        {
            IEnumerable<Product> query = _context.Products
                                   .Where(x => x.DeletedAt == null)
                                   .Include(x => x.ProductImages)
                                   .Include(x => x.Category)
                                   .Include(x => x.ProductColors)
                                   .ThenInclude(x => x.Color)
                                   .Include(x => x.ProductColors)
                                   .ThenInclude(x => x.ProductColorSizes)
                                   .ThenInclude(x => x.Size)
                                   .AsEnumerable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Name.ToLower().Contains(search.ToLower()));
;           }

            if(!string.IsNullOrEmpty(color) && color != "color-all") // ?color=1,3 
            {
                string[] colorArr = color.Split(','); // [1,3]
                query = query.Where(x => x.ProductColors.Any(p => colorArr.Contains(p.ColorId.ToString())));
            }

            if (!string.IsNullOrEmpty(category) && category != "category-all")
            {
                string[] categoryArr = category.Split(',');
                query = query.Where(x => categoryArr.Contains(x.CategoryId.ToString()));
            }

            if (!string.IsNullOrEmpty(size) && size != "size-all")
            {
                string[] sizeArr = size.Split(",");
                query = query.Where(x => x.ProductColors.Any(x => x.ProductColorSizes.Any(x => sizeArr.Contains(x.SizeId.ToString()))));
            }

            if(min != null && max != null)
            {
                query = query.Where(x => x.SalePrice - x.Discount >= min && x.SalePrice - x.Discount <= max);
            }

            return query.ToList();
        }

        public async Task<List<Product>> GetAllProductsWithDetails()
        {
            return await _context.Products
                           .Where(x=>x.DeletedAt == null)
                           .Include(x=>x.ProductImages)
                           .Include(x=>x.Category)
                           .Include(x=>x.ProductColors)
                           .ThenInclude(x=>x.Color)
                           .Include(x=>x.ProductColors)
                           .ThenInclude(x=>x.ProductColorSizes)
                           .ThenInclude(x=>x.Size)
                           .ToListAsync();
        }

        public async Task<Product> GetProductBySlug(string slug)
        {
            return await _context.Products
                        .Include(x => x.ProductColors)
                        .ThenInclude(x => x.Color)
                        .Include(x => x.ProductColors)
                        .ThenInclude(x => x.ProductColorSizes)
                        .ThenInclude(x => x.Size)
                        .Include(x => x.ProductImages)
                        .FirstOrDefaultAsync(x => x.Slug == slug);
        }

        public async Task<ProductColor> GetProductColor(int id)
        {
            return await _context.ProductColors
                .Include(x=>x.ProductColorSizes)
                .ThenInclude(x=>x.Size)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ProductColorSize> GetProductColorSize(int id)
        {
            return await _context.ProductColorSizes
                .Include(x=>x.Size)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Product> GetProductWithDetails(int id)
        {
            return await _context.Products
                           .Where(x => x.DeletedAt == null)
                           .Include(x => x.ProductImages)
                           .Include(x => x.Category)
                           .Include(x => x.ProductColors)
                           .ThenInclude(x => x.Color)
                           .Include(x => x.ProductColors)
                           .ThenInclude(x=>x.ProductColorSizes)
                           .ThenInclude(x=>x.Size)
                           .FirstOrDefaultAsync(x=>x.Id == id);
        }

        public void RemoveImages(Product product, ImageStatus status)
        {
            List<ProductImage> productImages = product.ProductImages.Where(x => x.Status == status).ToList();
            foreach (ProductImage image in productImages)
            {
                _context.ProductImages.Remove(image);
            }
        }

        public async Task Update(Product product, ProductPutDTO model)
        {
            product.Name = model.Name;
            product.SalePrice = model.SalePrice;
            product.Discount = (double)model.Discount;
            product.CategoryId = model.CategoryId;
            product.UpdatedAt = DateTime.Now;

            // colors - 1,2,3,4
            // old id - 1,3
            // selected - 3,4
            // remove - 1
            // add - 4
            List<int> oldIds = product.ProductColors.Select(x => x.ColorId).ToList();

            List<int> removeIds = oldIds.FindAll(x => !model.ColorIds.Contains(x));

            List<int> addIds = model.ColorIds.FindAll(x => !oldIds.Contains(x));

            product.ProductColors.RemoveAll(x => removeIds.Contains(x.ColorId));
            this.AddColors(product, addIds);



            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductColorSize(ProductColorSize productColorSize, ProductColorSizeDTO model)
        {
            productColorSize.Count = model.Count;
            await _context.SaveChangesAsync();
        }
    }
}
