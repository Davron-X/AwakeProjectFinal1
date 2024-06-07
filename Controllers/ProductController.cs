using Awake_Data.Db;
using Awake_Data.Repository.IRepository;
using Awake_Models;
using Awake_Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AwakeProject.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ProductController : Controller
    {
        private IProductRepository productRepo;
        private IWebHostEnvironment webHostEnvironment;
        public ProductController(IProductRepository productRepository,IWebHostEnvironment hostEnvironment)
        {
            productRepo = productRepository;
            webHostEnvironment = hostEnvironment;
        }

        public IActionResult Index()        
        {
            var products = productRepo.GetAll(includeProperties:"Category,ApplicationType").ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            ProductVM productVM = new()
            {
                Categories = productRepo.GetAllDropDownList(WC.CategoryName),
                AppTypes = productRepo.GetAllDropDownList(WC.AppTypeName)              
            };
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductVM productVM)
        {
            if (Request.Form.Files.Count<=0)
            {
                ModelState.AddModelError("", "Pls upload image");
            }
            if (ModelState.IsValid)
            {
                IFormFileCollection files= Request.Form.Files;

                string fileName = Guid.NewGuid().ToString()+ Path.GetExtension(files[0].FileName);
                string path = Path.Combine(WC.ProductImgPath,fileName);
                string web = webHostEnvironment.WebRootPath;
                using(var filestream=new FileStream(webHostEnvironment.WebRootPath+path, FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }
                productVM.Product.Image = fileName;
                productRepo.Add(productVM.Product);
                productRepo.Save();
                TempData[WC.Success] = "Продукт успешно добавлен";
                return RedirectToAction(nameof(Index));
            }

            productVM.Categories = productRepo.GetAllDropDownList(WC.CategoryName);
            productVM.AppTypes = productRepo.GetAllDropDownList(WC.AppTypeName);
            return View(productVM);
        }


        public IActionResult Edit(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var product = productRepo.Find(id);
            if (product == null)
                return NotFound();

            ProductVM productVM = new()
            {
                Product=product,
                Categories = productRepo.GetAllDropDownList(WC.CategoryName),
                AppTypes = productRepo.GetAllDropDownList(WC.AppTypeName)
            };
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                Product product = productRepo.FirstOrDefault(x => x.Id == productVM.Product.Id,isTracking:false);
                productVM.Product.Image = product.Image;
                if (Request.Form.Files.Count > 0)
                {
                    if (product.Image != null)
                    {
                        string pathOldFile = webHostEnvironment.WebRootPath + Path.Combine(WC.ProductImgPath, product.Image);
                        if (System.IO.File.Exists(pathOldFile))
                        {
                            System.IO.File.Delete(pathOldFile);
                        }
                    }
                    IFormFileCollection files = Request.Form.Files;

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(files[0].FileName);
                    string path = Path.Combine(WC.ProductImgPath, fileName);
                    string web = webHostEnvironment.WebRootPath;
                    using (var filestream = new FileStream(webHostEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }

                    productVM.Product.Image = fileName;
                }

                productRepo.Update(productVM.Product);
                productRepo.Save();
                return RedirectToAction(nameof(Index));
            }

            productVM.Categories = productRepo.GetAllDropDownList(WC.CategoryName);
            productVM.AppTypes = productRepo.GetAllDropDownList(WC.AppTypeName);
            TempData[WC.Success] = "Продукт успешно изменён";
            return View(productVM);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = productRepo.FirstOrDefault(x=>x.Id==id,includeProperties:"ApplicationType,Category");
            if (product == null)
                return NotFound();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Product product)
        {
            Product prod = productRepo.FirstOrDefault(x => x.Id == product.Id);
            if (prod == null)
                return NotFound();

            if (prod.Image != null)
            {
                string pathOldFile = webHostEnvironment.WebRootPath + Path.Combine(WC.ProductImgPath, prod.Image);
                if (System.IO.File.Exists(pathOldFile))
                {
                    System.IO.File.Delete(pathOldFile);
                }
            }

            productRepo.Remove(prod);
            productRepo.Save();
            TempData[WC.Success] = "Продукт успешно удалён";
            return RedirectToAction(nameof(Index));
        }

    }
}
