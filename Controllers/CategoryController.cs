using Awake_Data.Db;
using Awake_Data.Repository.IRepository;
using Awake_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AwakeProject.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class CategoryController : Controller
    {
        private ICategoryRepository categoryRepository;
        public CategoryController(ICategoryRepository categoryRep)
        {
            categoryRepository = categoryRep;
        }

        public IActionResult Index()        
        {
            var caregories = categoryRepository.GetAll().ToList();
            return View(caregories);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Add(category);
                categoryRepository.Save();
                TempData[WC.Success] = "Категория успешно добавлена";
                return RedirectToAction(nameof(Index));
            }
            TempData[WC.Error] = "Не удалось добавь категорию";
            return View(category);
        }

        public IActionResult Edit(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var caregory= categoryRepository.Find(id);

            if (caregory==null)
            {
                return NotFound();
            }
            return View( caregory);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Update(category);
                categoryRepository.Save();
                TempData[WC.Success] = "Категория успешно изменена";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public IActionResult Delete(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var caregory= categoryRepository.Find(id);

            if (caregory==null)
            {
                return NotFound();
            }
            return View(caregory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var category= categoryRepository.Find(id);
            if (category==null)
            {
                return NotFound();
            }
            categoryRepository.Remove(category);
            categoryRepository.Save();
            TempData[WC.Success] = "Категория успешно удалена";
            return RedirectToAction(nameof(Index));
        }
    }
}
