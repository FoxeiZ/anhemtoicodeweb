using System.Collections.Generic;
using System.Linq;

namespace anhemtoicodeweb.Models
{
    public class CartItem
    {
        public int _quantity { get; set; }
        public Product _product { get; set; }
    }

    public class Cart
    {
        private readonly List<CartItem> items = new List<CartItem>();
        public IEnumerable<CartItem> Items
        {
            get { return items; }
        }

        public bool AddProductCart(Product product, int quantity = 1)
        {
            var item = Items.FirstOrDefault(s => s._product.ProductID == product.ProductID);
            if (item == null)
            {
                items.Add(new CartItem
                {
                    _quantity = quantity,
                    _product = product
                });
                return true;
            }

            var tempQ = quantity + item._quantity;
            if (tempQ > product.InvQuantity)
            {
                item._quantity = product.InvQuantity;
                return false;
            }
            item._quantity = tempQ;
            return true;
        }

        public int TotalQuantity()
        {
            return items.Sum(s => s._quantity);
        }

        public decimal TotalMoney()
        {
            var total = items.Sum(s => s._quantity * s._product.Price);
            return total;
        }

        public void UpdateQuantity(int id, int new_quantity)
        {
            var item = Items.FirstOrDefault(s => s._product.ProductID == id);
            if (item != null)
                if (new_quantity == 0)
                {
                    RemoveCartItem(id);
                    return;
                }
            item._quantity = new_quantity;
        }

        public void RemoveCartItem(int id)
        {
            items.RemoveAll(s => s._product.ProductID == id);
        }

        public void ClearCart()
        {
            items.Clear();
        }
    }
}