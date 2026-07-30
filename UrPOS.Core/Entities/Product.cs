using System;

namespace UrPOS.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int MinStockLevel { get; set; }
        public int CurrentStock { get; set; }

        /// <summary>وحدة القياس (قطعة، صندوق، عبوة، ...).</summary>
        public string UnitOfMeasure { get; set; } = "قطعة";

        // حقل الـ JSONB لتخزين الخصائص الديناميكية للموديولات (صيدلية، سوبرماركت)
        public string? CustomAttributes { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public static class ProductUnits
    {
        public static readonly string[] Common =
        [
            "قطعة",
            "صندوق",
            "عبوة",
            "كيلو",
            "لتر",
            "متر",
            "رزمة"
        ];
    }
}