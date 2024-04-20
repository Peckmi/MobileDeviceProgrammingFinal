using System;
using Xamarin.Forms;

namespace MobileDeviceProgrammingFinal.Models
{
    public class InventoryItem : BindableObject
    {
        private int _quantityToOrder;
        public string ItemId { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public int QuantityToOrder
        {
            get => _quantityToOrder;
            set
            {
                if (_quantityToOrder != value)
                {
                    _quantityToOrder = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
