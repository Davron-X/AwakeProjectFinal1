using Awake_Data.Db;
using Awake_Data.Repository.IRepository;
using Awake_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AwakeProject.Controllers
{
    [Authorize(Roles =WC.AdminRole)]
    public class ApplicationTypeController : Controller
    {
        private IApplicationTypeRepository appTypeRepo;
        public ApplicationTypeController(IApplicationTypeRepository applicationTypeRepository)
        {
            appTypeRepo = applicationTypeRepository;
        }

        public IActionResult Index()        
        {
            var appTypes = appTypeRepo.GetAll().ToList();
            return View(appTypes);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationType applicationType)
        {
            if (ModelState.IsValid)
            {
                appTypeRepo.Add(applicationType);
                appTypeRepo.Save();
                TempData[WC.Success] = "Тип успешно добавлен";
                return RedirectToAction(nameof(Index));
            }
            return View(applicationType);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var applicationType = appTypeRepo.Find(id);

            if (applicationType == null)
            {
                return NotFound();
            }
            return View(applicationType);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType applicationType)
        {
            if (ModelState.IsValid)
            {
                appTypeRepo.Update(applicationType);
                appTypeRepo.Save();
                TempData[WC.Success] = "Тип успешно изменён";
                return RedirectToAction(nameof(Index));
            }
            return View(applicationType);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var applicationType = appTypeRepo.Find(id);

            if (applicationType == null)
            {
                return NotFound();
            }
            return View(applicationType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var applicationType = appTypeRepo.Find(id);
            if (applicationType == null)
            {
                return NotFound();
            }
            appTypeRepo.Remove(applicationType);
            appTypeRepo.Save();
            TempData[WC.Success] = "Тип успешно удалён";
            return RedirectToAction(nameof(Index));
        }
    }
}
