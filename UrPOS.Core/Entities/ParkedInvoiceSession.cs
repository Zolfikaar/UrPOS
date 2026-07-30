using System;
using System.Collections.Generic;
using System.Linq;

namespace UrPOS.Core.Entities
{
    /// <summary>
    /// Session-scoped parked/pending POS carts that survive closing the cashier window.
    /// </summary>
    public sealed class ParkedInvoiceDraft
    {
        public string Title { get; set; } = string.Empty;
        public DateTime ParkedAt { get; set; }
        public List<SalesInvoiceItem> Items { get; set; } = new();
    }

    public sealed class ParkedInvoiceSession
    {
        private static ParkedInvoiceSession? _instance;
        private static readonly object _lock = new();

        private readonly List<ParkedInvoiceDraft> _drafts = new();
        private int _draftSequence;

        private ParkedInvoiceSession()
        {
        }

        public static ParkedInvoiceSession Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new ParkedInvoiceSession();
                }
            }
        }

        public IReadOnlyList<ParkedInvoiceDraft> Drafts
        {
            get
            {
                lock (_lock)
                {
                    return _drafts.ToList();
                }
            }
        }

        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _drafts.Count;
                }
            }
        }

        public ParkedInvoiceDraft Park(IEnumerable<SalesInvoiceItem> items)
        {
            var cloned = items.Select(CloneItem).ToList();
            if (cloned.Count == 0)
            {
                throw new InvalidOperationException("Cannot park an empty cart.");
            }

            lock (_lock)
            {
                _draftSequence++;
                var draft = new ParkedInvoiceDraft
                {
                    Title = $"مسودة #{_draftSequence} — {cloned.Count} صنف",
                    ParkedAt = DateTime.Now,
                    Items = cloned
                };
                _drafts.Add(draft);
                return draft;
            }
        }

        public ParkedInvoiceDraft? Take(int index)
        {
            lock (_lock)
            {
                if (index < 0 || index >= _drafts.Count)
                {
                    return null;
                }

                var draft = _drafts[index];
                _drafts.RemoveAt(index);
                return draft;
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _drafts.Clear();
                _draftSequence = 0;
            }
        }

        private static SalesInvoiceItem CloneItem(SalesInvoiceItem item)
        {
            return new SalesInvoiceItem
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Barcode = item.Barcode,
                Quantity = item.Quantity,
                SalePrice = item.SalePrice,
                TotalPrice = item.TotalPrice
            };
        }
    }
}
