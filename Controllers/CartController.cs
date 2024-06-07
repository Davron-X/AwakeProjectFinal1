using Awake_Data.Db;
using Awake_Data.Repository.IRepository;
using Awake_Models;
using Awake_Models.ViewModel;
using AwakeProject.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AwakeProject.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private IApplicationUserRepository applicationUserRepo;
        private IProductRepository productRepo;
        private IInquiryDetailRepository inquiryDetailRepo;
        private IInquiryHeaderRepository inquiryHeaderRepo;

        public CartController(IInquiryHeaderRepository inquiryHeaderRepo, IApplicationUserRepository applicationUserRepo, IProductRepository productRepo, IInquiryDetailRepository inquiryDetailRepo)
        {
            this.applicationUserRepo = applicationUserRepo;
            this.productRepo = productRepo;
            this.inquiryDetailRepo = inquiryDetailRepo;
            this.inquiryHeaderRepo = inquiryHeaderRepo;
        }
        public IActionResult Index()
        {
            List<ShoppingCartVM> shoppingCartVMs = new();
            if (HttpContext.Session.Get<List<ShoppingCartVM>>(WC.ShoppingCartKey)!=null
                && HttpContext.Session.Get<List<ShoppingCartVM>>(WC.ShoppingCartKey)!.Count>0)
            {
                shoppingCartVMs = HttpContext.Session.Get<List<ShoppingCartVM>>(WC.ShoppingCartKey)!;
            }
            List<int> prodsId = shoppingCartVMs.Select(x => x.ProductId).ToList();
            List<Product> products = productRepo.GetAll(y => prodsId.Contains(y.Id)).ToList();
            List<Product> viewProducts = new();
            foreach (var item in shoppingCartVMs)
            {
               Product product=  products.FirstOrDefault(x => x.Id == item.ProductId);
                product.TempQuantity = item.Quantity;
                viewProducts.Add(product);
            }

            return View(viewProducts);
        }
        [HttpPost]
        [ActionName("Index")]
        public IActionResult IndexPost(List<Product> products)
        {
            List<ShoppingCartVM> shoppingCartList = new();
            foreach (var item in products)
            {
                shoppingCartList.Add(new ShoppingCartVM { ProductId = item.Id, Quantity = item.TempQuantity });
            }
            HttpContext.Session.Set(shoppingCartList, WC.ShoppingCartKey);
            return RedirectToAction(nameof(Summary));
        }
        public IActionResult Summary()
         {
            var claimsIdentity= (ClaimsIdentity)User.Identity;
           Claim nameId=  claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<ShoppingCartVM> shoppingCartVMs = new();
            if (HttpContext.Session.Get<List<ShoppingCartVM>>(WC.ShoppingCartKey) != null
                && HttpContext.Session.Get<List<ShoppingCartVM>>(WC.ShoppingCartKey)!.Count > 0)
            {
                shoppingCartVMs = HttpContext.Session.Get<List<ShoppingCartVM>>(WC.ShoppingCartKey)!;
            }
            List<int> prodsId = shoppingCartVMs.Select(x => x.ProductId).ToList();
            List<Product> products = productRepo.GetAll(y => prodsId.Contains(y.Id)).ToList();
            UserProductsVM userProductsVM = new()
            {               
                User = applicationUserRepo.FirstOrDefault(x => x.Id == nameId.Value)
            };

            foreach (var item in shoppingCartVMs)
            {
                Product product = productRepo.FirstOrDefault(x => x.Id == item.ProductId);
                product.TempQuantity = item.Quantity;
                userProductsVM.Products.Add(product);
            }

            return View(userProductsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPost(UserProductsVM userProductsVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            InquiryHeader inquiryHeader = new()
            {
                AppUserId = claim.Value,
                FullName = userProductsVM.User.FullName,
                Email = userProductsVM.User.Email,
                PhoneNumber = userProductsVM.User.PhoneNumber,
                InquireDate=DateTime.Now,
                City=userProductsVM.User.City,
                State=userProductsVM.User.State,
                StreetAddress=userProductsVM.User.StreetAddress,
                Status=WC.Processing
            };
            inquiryHeaderRepo.Add(inquiryHeader);
            inquiryHeaderRepo.Save();

            List<ShoppingCartVM> shoppingCartVMs = new();
            if (HttpContext.Session.Get<List<ShoppingCartVM>>(WC.ShoppingCartKey) != null
                && HttpContext.Session.Get<List<ShoppingCartVM>>(WC.ShoppingCartKey)!.Count > 0)
            {
                shoppingCartVMs = HttpContext.Session.Get<List<ShoppingCartVM>>(WC.ShoppingCartKey)!;
            }
         

            foreach (var item in userProductsVM.Products)
            {
                InquiryDetail inquiryDetail = new()
                {
                    InquiryHeaderId = inquiryHeader.Id,
                    ProductId=item.Id,
                    ProdQuantity=shoppingCartVMs.Where(x=>x.ProductId==item.Id).Select(x => x.Quantity).FirstOrDefault()
                };
                inquiryDetailRepo.Add(inquiryDetail);
            }
            inquiryDetailRepo.Save();
            TempData[WC.Success] = "Заказ оформлен";


            return RedirectToAction(nameof(Clear));
        }
        public IActionResult Remove(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            List<ShoppingCartVM> shoppingCartVMs = new();
            if (HttpContext.Session.Get<List<ShoppingCartVM>>(WC.ShoppingCartKey)!=null
                && HttpContext.Session.Get<List<ShoppingCartVM>>(WC.ShoppingCartKey)!.Count>0)
            {
                shoppingCartVMs = HttpContext.Session.Get<List<ShoppingCartVM>>(WC.ShoppingCartKey)!;
            }
            shoppingCartVMs.Remove(shoppingCartVMs.FirstOrDefault(x => x.ProductId==id));

            HttpContext.Session.Set(shoppingCartVMs, WC.ShoppingCartKey);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult UpdateCart(IEnumerable<Product> products)
        {
            List<ShoppingCartVM> shoppingCartList = new();
            foreach (var item in products)
            {
                shoppingCartList.Add(new ShoppingCartVM { ProductId = item.Id, Quantity = item.TempQuantity });
            }
            HttpContext.Session.Set(shoppingCartList,WC.ShoppingCartKey);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Clear(int? id)
        {

            HttpContext.Session.Clear() ;
            return RedirectToAction("Index","Home");
        }

    }
}
