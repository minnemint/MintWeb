using Microsoft.AspNetCore.Mvc;
using Mint.DataAccess.Repository.IRepository;
using Mint.Models;
using System.Diagnostics;

namespace Mint.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productsList = _unitOfWork.Product.GetAll(includeProperties: "Category"); 
            return View(productsList);
        }
        public IActionResult Details(int productid)
        {
            Product product = _unitOfWork.Product.Get(u => u.Id == productid, includeProperties: "Category");

            if (product == null)
            {
                return NotFound(); // Or however you want to handle this.
            }

            return View(product);
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
