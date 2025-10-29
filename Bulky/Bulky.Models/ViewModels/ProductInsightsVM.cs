using System.Collections.Generic;

namespace BulkyBook.Models.ViewModels
{
    public class ProductInsightsVM
    {
        public int TotalProducts { get; set; }
        public double? AveragePrice { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public IReadOnlyCollection<ProductCategoryInsight> CategoryBreakdown { get; set; } = new List<ProductCategoryInsight>();
        public IReadOnlyCollection<ProductPriceDistribution> PriceDistribution { get; set; } = new List<ProductPriceDistribution>();
    }

    public class ProductCategoryInsight
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int ProductCount { get; set; }
        public double AveragePrice { get; set; }
    }

    public class ProductPriceDistribution
    {
        public string Label { get; set; } = string.Empty;
        public int ProductCount { get; set; }
    }
}
