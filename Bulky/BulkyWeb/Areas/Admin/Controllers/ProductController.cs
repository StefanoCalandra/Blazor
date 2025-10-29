using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAcess.Data;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private const long MaxImageFileSizeBytes = 2 * 1024 * 1024;
        private static readonly HashSet<string> AllowedImageExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".gif",
            ".webp"
        };

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var viewModel = new ProductListVM
            {
                CategoryList = GetCategorySelectList()
            };

            return View(viewModel);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = GetCategorySelectList(),
                Product = new Product(),
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
                if (productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            NormalizeProduct(productVM.Product);
            ValidateProductPricing(productVM.Product);
            ValidateUniqueIsbn(productVM.Product);
            if (file != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, "images", "product");

                    if (!Directory.Exists(productPath))
                    {
                        Directory.CreateDirectory(productPath);
                    }

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //delete the old image
                        var oldImagePath =
                            Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\', '/'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = $"/images/product/{fileName}";
                }

                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
                
                _unitOfWork.Save();
                TempData["success"] = productVM.Product.Id == 0
                    ? "Product created successfully"
                    : "Product updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                _unitOfWork.Product.Update(productVM.Product);
                TempData["success"] = "Product updated successfully";
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll(int? categoryId, string? searchTerm, double? minPrice, double? maxPrice)
        {
            var products = _unitOfWork.Product.GetAll(includeProperties: "Category").AsEnumerable();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
            {
                (minPrice, maxPrice) = (maxPrice, minPrice);
            }

            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var normalizedTerm = searchTerm.Trim();
                products = products.Where(p =>
                    (!string.IsNullOrWhiteSpace(p.Title) && p.Title.Contains(normalizedTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(p.Author) && p.Author.Contains(normalizedTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(p.ISBN) && p.ISBN.Contains(normalizedTerm, StringComparison.OrdinalIgnoreCase)));
            }

            return Json(new { data = products.ToList() });
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int? id, CancellationToken cancellationToken = default)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }

            var oldImagePath =
                           Path.Combine(_webHostEnvironment.WebRootPath,
                           productToBeDeleted.ImageUrl.TrimStart('\\', '/'));

            if (System.IO.File.Exists(oldImagePath))
            {
                return Json(new { success = false, message = "Product not found." });
            }

            DeleteImage(productToBeDeleted.ImageUrl);

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Product deleted successfully." });
        }

        #endregion

        private IEnumerable<SelectListItem> GetCategorySelectList()
        {
            return _unitOfWork.Category.GetAll()
                .OrderBy(u => u.DisplayOrder)
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
        }

        private void NormalizeProduct(Product product)
        {
            if (product == null)
            {
                return;
            }

            product.Title = product.Title?.Trim();
            product.Description = product.Description?.Trim();
            product.Author = product.Author?.Trim();
            product.ISBN = product.ISBN?.Trim();
        }

        private void ValidateProductPricing(Product product)
        {
            if (product == null)
            {
                return;
            }

            if (product.ListPrice < product.Price)
            {
                ModelState.AddModelError("Product.ListPrice", "List price must be greater than or equal to the price for 1-50.");
            }

            if (product.Price < product.Price50)
            {
                ModelState.AddModelError("Product.Price", "Price for 1-50 must be greater than or equal to the price for 50+.");
            }

            if (product.Price50 < product.Price100)
            {
                ModelState.AddModelError("Product.Price50", "Price for 50+ must be greater than or equal to the price for 100+.");
            }
        }

        private void ValidateUniqueIsbn(Product product)
        {
            if (product == null || string.IsNullOrWhiteSpace(product.ISBN))
            {
                return;
            }

            var duplicateProduct = _unitOfWork.Product.Get(u => u.ISBN == product.ISBN && u.Id != product.Id);
            if (duplicateProduct != null)
            {
                ModelState.AddModelError("Product.ISBN", "A product with the same ISBN already exists.");
            }
        }

        private void ValidateImageFile(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            if (!AllowedImageExtensions.Contains(extension))
            {
                ModelState.AddModelError("Product.ImageUrl", "Only JPG, PNG, GIF, and WEBP images are allowed.");
            }

            if (file.Length > MaxImageFileSizeBytes)
            {
                ModelState.AddModelError("Product.ImageUrl", "Image size must be 2 MB or less.");
            }
        }

        private string SaveImage(IFormFile file, string wwwRootPath)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string productPath = Path.Combine(wwwRootPath, "images", "product");

            if (!Directory.Exists(productPath))
            {
                Directory.CreateDirectory(productPath);
            }

            using var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create);
            file.CopyTo(fileStream);

            return fileName;
        }

        private void DeleteImage(string? imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return;
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                imageUrl.TrimStart('\\', '/'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
        }
    }
}
