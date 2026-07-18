using System;

namespace UrPOS.Core.Entities
{
    public class Supplier
    {
        public int Id { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public string? Phone { get; set; }

        // رصيد المورد: قيمة موجبة تعني نطلب منه بضاعة، سالبة تعني يطلبنا فلوس
        public decimal Balance { get; set; } = 0.000m;
    }
}