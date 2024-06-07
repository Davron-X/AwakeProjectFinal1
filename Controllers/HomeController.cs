using Awake_Data.Db;
using Awake_Data.Repository.IRepository;
using Awake_Models;
using Awake_Models.ViewModel;
using AwakeProject.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AwakeProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository productRepo;
        private readonly ICategoryRepository categoryRepo;
        public HomeController( IProductRepository prouctRepo = null, ICategoryRepository categoryRepo = null)
        {
            this.productRepo = prouctRepo;
            this.categoryRepo = categoryRepo;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new()
            {
                Categories = categoryRepo.GetAll(),
                Products = productRepo.GetAll(includeProperties:"Category,ApplicationType")
            };
            return View(homeVM);
        }

        public IActionResult Details(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            List<ShoppingCartVM> shoppingCarts = new();
            if (HttpContext.Session.Get<List<ShoppingCartVM>>(WC.ShoppingCartKey) != null
                && HttpContext.Session.Get<List<ShoppingCartVM>>(WC.ShoppingCartKey)!.Count() > 0)
            {
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCartVM>>(WC.ShoppingCartKey)!;
            }

            DetailsVM detailsVM = new()
            {
                ExistInCar = false,
                Product =productRepo.FirstOrDefault( x=> x.Id == id, includeProperties: "Category,ApplicationType")
            };

            foreach (var item in shoppingCarts)
            {
                if (item.ProductId == detailsVM.Product.Id)
                    detailsVM.ExistInCar = true;
            }
            return View(detailsVM);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddToCart(int id,DetailsVM detailsVM)
        {
            List<ShoppingCartVM> shoppingCarts = new();
            if (HttpContext.Session.Get<List<ShoppingCartVM>>(WC.ShoppingCartKey)!=null
                && HttpContext.Session.Get<List<ShoppingCartVM>>(WC.ShoppingCartKey)!.Count()>0)
            {
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCartVM>>(WC.ShoppingCartKey)!;
            }
            shoppingCarts.Add(new ShoppingCartVM() { ProductId=id,Quantity=detailsVM.Product.TempQuantity});

            HttpContext.Session.Set(shoppingCarts, WC.ShoppingCartKey);
            TempData[WC.Success] = "Товар добавлен в корзину";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult RemoveFromCart(int? id)
        {
            if (id == null)
                return NotFound();
            List<ShoppingCartVM> shoppingCarts = new();
            if (HttpContext.Session.Get<List<ShoppingCartVM>>(WC.ShoppingCartKey)!=null
                && HttpContext.Session.Get<List<ShoppingCartVM>>(WC.ShoppingCartKey)!.Count()>0)
            {
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCartVM>>(WC.ShoppingCartKey)!;
            }
            var prod = shoppingCarts.SingleOrDefault(x => x.ProductId == id);
            shoppingCarts.Remove(prod!);

            HttpContext.Session.Set(shoppingCarts, WC.ShoppingCartKey);
            TempData[WC.Success] = "Товар удалён из корзины";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
