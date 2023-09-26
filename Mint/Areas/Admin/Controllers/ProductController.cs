using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Mint.DataAccess.Repository.IRepository;
using Mint.Models;
using Mint.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Mint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work instance.</param>
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Displays the list of products.
        /// </summary>
        public IActionResult Index()
        {
            List<Product> product = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return View(product);
        }

        /// <summary>
        /// Displays the product creation form.
        /// </summary>
        public IActionResult Upsert(int? id)
        {

            //IEnumerable<SelectListItem> CategoryList = 
            //ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"] = CategoryList;

            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {

                    Text = u.Name,
                    Value = u.Id.ToString()

                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
        }

        /// <summary>
        /// Handles the product creation post request.
        /// </summary>
        /// <param name="product">The product to create.</param>
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //Delete the old images
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var filestream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }

                    productVM.Product.ImageUrl = @"\images\product\" + filename;
                }
                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
                //_unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        ///// <summary>
        ///// Displays the product deletion form.
        ///// </summary>
        ///// <param name="id">The product ID.</param>
        //public IActionResult Delete(int? id)
        //{
        //    if (id is null or 0)
        //        return NotFound();

        //    var productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
        //    if (productFromDb == null)
        //        return NotFound();

        //    return View(productFromDb);
        //}

        /// <summary>
        /// Handles the product deletion post request.
        /// </summary>
        /// <param name="id">The product ID.</param>
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int? id)
        {
            var product = _unitOfWork.Product.Get(u => u.Id == id);
            if (product == null)
                return NotFound();

            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();
            TempData["success"] = "Deleted successfully";
            return RedirectToAction(nameof(Index));
        }
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll(int? id) {
            List<Product> product = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = product });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            string productPath = @"images\products\product-" + id;
            string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);

            if (Directory.Exists(finalPath))
            {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }

                Directory.Delete(finalPath);
            }


            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}