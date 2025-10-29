using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models.ViewModels
{
    public class ProductListVM
    {
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; } = new List<SelectListItem>();

        [Display(Name = "Category")]
        public int? SelectedCategoryId { get; set; }

        [Display(Name = "Min price")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to zero.")]
        public double? MinPrice { get; set; }

        [Display(Name = "Max price")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to zero.")]
        public double? MaxPrice { get; set; }

        [Display(Name = "Search")] 
        public string? SearchTerm { get; set; }
    }
}
