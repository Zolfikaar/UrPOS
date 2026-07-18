using System;

namespace UrPOS.Core.Entities
{
    public class InventoryMovement
    {
        public long Id { get; set; }
        public int ProductId { get; set; }
        public string MovementType { get; set; } = string.Empty; // IN, OUT, ADJUST, RETURN
        public int Quantity { get; set; }
        public long? ReferenceId { get; set; } // رقم فاتورة البيع أو الشراء المرتبطة بالحركة
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation Property للإستعلامات (اختياري حسب الحاجة في العرض)
        public string? ProductName { get; set; }
    }
}