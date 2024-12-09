using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PB303Fashion.DataAccessLayer;
using PB303Fashion.DataAccessLayer.Entities;
using PB303Fashion.Models;

namespace PB303Fashion.Areas.AdminPanel.Controllers
{
    public class CategoryController : AdminController
    {
        private readonly AppDbContext _dbContext;

        public CategoryController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _dbContext.Categories.ToListAsync();

            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModal modal)
        {
            var exist = await _dbContext.Categories.AnyAsync(x => x.Name == modal.Name);

            if (exist)
            {
                ModelState.AddModelError("Name", "Category already exist");
                return View();
            }
         
            var category = new Category()
            {
               Name = modal.Name,
               ImageUrl = "2.jpg"
            };
            await _dbContext.Categories.AddAsync(category);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
            
        }
    }
}
