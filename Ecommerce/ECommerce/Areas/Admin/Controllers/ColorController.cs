using AutoMapper;
using Ecommerce.Data.DTOs.ColorDTO;
using Ecommerce.Data.Entities;
using Ecommerce.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ColorController : Controller
    {
        private readonly IColorRepository _colorRepository;
        private readonly IMapper _mapper;
        public ColorController(IColorRepository colorRepository,IMapper mapper)
        {
            _colorRepository = colorRepository;
            _mapper = mapper;
        }
        // GET: ColorController
        public async Task<ActionResult> Index()
        {
            List<Color> colors = await _colorRepository.GetAllAsync();
            var data = _mapper.Map<List<ColorGetDTO>>(colors);
            return View(data);
        }

        // GET: ColorController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ColorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ColorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ColorPostDTO model)
        {
            try
            {
                Color color = _mapper.Map<Color>(model);
                await _colorRepository.CreateAsync(color);
                TempData["Message"] = "Color has been successfully created";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GET: ColorController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Color color = await _colorRepository.GetByIdAsync(id);
            var data = _mapper.Map<ColorPostDTO>(color);
            return View(data);
        }

        // POST: ColorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ColorPostDTO model)
        {
            try
            {
                Color color = await _colorRepository.GetByIdAsync(id);
                await _colorRepository.UpdateAsync(color, model);
                TempData["Message"] = "Color has been successfully updated";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GET: ColorController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Color color = await _colorRepository.GetByIdAsync(id);
                _colorRepository.Delete(color);
                return Json(new
                {
                    Status = true,
                    Message = "Color successfully deleted",
                });

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Status = false,
                    Message = ex.Message,
                });
                
            }
        }
    }
}
