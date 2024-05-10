using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DataAccesLayer;
using Pronia.Models;
using Pronia.ViewModels.Categories;
using Pronia.ViewModels.Sliders;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController(ProniaContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var data = await _context.Categories.Select(c => new GetCategoryVM
            {
                Name=c.Name,
            }).
            ToListAsync();
            return View(data ?? new List<GetCategoryVM>());
           }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryVM Createvm)
        {
            if (!ModelState.IsValid)
            {
                return View(Createvm);
            }
            Category category = new Category
            {

                CreatedTime = DateTime.Now,
                IsDeleted = false,
                Id = Createvm.Id,
                Name = Createvm.Name
            };
            await _context.Categories.AddAsync(category);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) return BadRequest();

            Category category = await _context.Categories.FirstOrDefaultAsync(s => s.Id == id);
            if (category is null) return NotFound();

            UpdateCategoryVM categoryVM = new UpdateCategoryVM
            {
                Name = category.Name


            };

            return View(categoryVM);
        }

    }
}
