using AutoMapper;
using Ecommerce.Data.DTOs.BasketDTO;
using Ecommerce.Data.DTOs.CategoryDTO;
using Ecommerce.Data.Entities;
using Ecommerce.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Implementations
{
    public class LayoutService
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBasketRepository _basketRepository;
        public LayoutService(IMapper mapper,ICategoryRepository categoryRepository, IBasketRepository basketRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _basketRepository = basketRepository;
        }
        public async Task<List<CategoryGetDTO>> GetCategories()
        {
            List<Category> categories = await _categoryRepository.GetAllAsync();
            var data = _mapper.Map<List<CategoryGetDTO>>(categories);
            return data;
        }

        public List<BasketGetDTO> GetBasket()
        {
            return _basketRepository.GetAll();
        }
    }
}
