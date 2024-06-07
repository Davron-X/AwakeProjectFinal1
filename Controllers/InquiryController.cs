using Awake_Data.Repository.IRepository;
using Awake_Models;
using Awake_Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AwakeProject.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class InquiryController : Controller
    {
        private IInquiryDetailRepository inquiryDetailRepo;
        private IInquiryHeaderRepository inquiryHeaderRepo;
        [BindProperty]
        public InquiryVM InquiryVM { get; set; }
        public InquiryController(IInquiryDetailRepository inquiryDetailRepo, IInquiryHeaderRepository inquiryHeaderRepo)
        {
            this.inquiryDetailRepo = inquiryDetailRepo;
            this.inquiryHeaderRepo = inquiryHeaderRepo;
        }
        public IActionResult Index()
        {
            return View(); 
        }
        public IActionResult Details(int id)
        {
            InquiryVM = new()
            {
                InquiryHeader = inquiryHeaderRepo.FirstOrDefault(x => x.Id == id),
                InquiryDetails = inquiryDetailRepo.GetAll(x => x.InquiryHeaderId == id, includeProperties: "Product")
            };
            List<string> statuses = new List<string>() { WC.Processing, WC.Shipped, WC.Cencelled };
            IEnumerable<SelectListItem> selectLists = statuses.Select(x => new SelectListItem() { Text=x,Value = x });
            InquiryVM.Statuses = selectLists;
            return View(InquiryVM);
        }

        [HttpPost]
        public IActionResult Delete()
        {
            InquiryHeader inquiryHeader = inquiryHeaderRepo.FirstOrDefault(x => x.Id == InquiryVM.InquiryHeader.Id);
            IEnumerable<InquiryDetail> inquiryDetails = inquiryDetailRepo.GetAll(x => x.InquiryHeaderId == InquiryVM.InquiryHeader.Id);

            inquiryDetailRepo.RemoveRange(inquiryDetails);
            inquiryHeaderRepo.Remove(inquiryHeader);
            inquiryHeaderRepo.Save();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Update(InquiryVM inquiryVM)
        {
            inquiryHeaderRepo.Update(inquiryVM.InquiryHeader);
            inquiryHeaderRepo.Save();
            return RedirectToAction(nameof(Details),new {id= inquiryVM.InquiryHeader.Id } );
        }
        [HttpGet]
        public IActionResult GetInquiryList()
        {
            return Json(new { data = inquiryHeaderRepo.GetAll() });
        }
    }
}
