using AutoMapper;
using Ecommerce.Data.DTOs.SizeDTO;
using Ecommerce.Data.Entities;
using Ecommerce.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SizeController : Controller
    {
        private readonly ISizeRepository _sizeRepository;
        private readonly IMapper _mapper;
        public SizeController(ISizeRepository sizeRepository, IMapper mapper)
        {
            _sizeRepository = sizeRepository;
            _mapper = mapper;
        }
        // GET: SizeController
        public async Task<ActionResult> Index()
        {
            List<Size> sizes = await _sizeRepository.GetAllAsync();
            var data = _mapper.Map<List<SizeGetDTO>>(sizes);
            return View(data);
        }

        // GET: SizeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SizeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SizeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SizePostDTO model)
        {
            try
            {
                Size size = _mapper.Map<Size>(model);
                await _sizeRepository.CreateAsync(size);
                TempData["Message"] = "Size has been successfully created";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GET: SizeController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Size size = await _sizeRepository.GetByIdAsync(id);
            var data = _mapper.Map<SizePostDTO>(size);
            return View(data);
        }

        // POST: SizeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, SizePostDTO model)
        {
            try
            {
                Size size = await _sizeRepository.GetByIdAsync(id);
                await _sizeRepository.UpdateAsync(size, model);
                TempData["Message"] = "Size has been successfully updated";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GET: SizeController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Size size = await _sizeRepository.GetByIdAsync(id);
                _sizeRepository.Delete(size);
                return Json(new
                {
                    Status = true,
                    Message = "Size successfully deleted",
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
