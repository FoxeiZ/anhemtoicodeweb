using anhemtoicodeweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KetNoiDatabase.Models
{
    public class CartItem
    {
        public int _quantity { get; set; }
        public Product _product { get; set; }
    }

    public class Cart
    {
        List<CartItem> items = new List<CartItem>();
        public IEnumerable<CartItem> Items
        {
            get { return items; }
        }

        public void AddProductCart(Product product, int quantity = 1)
        {
            var item = Items.FirstOrDefault(s => s._product.ProductID == product.ProductID);
            if (item == null)
            {
                items.Add(new CartItem
                {
                    _quantity = quantity,
                    _product = product
                });
            }
            else
            {
                item._quantity += quantity;
            }
        }

        public int TotalQuantity()
        {
            return items.Sum(s => s._quantity);
        }

        public decimal TotalMoney()
        {
            var total = items.Sum(s => s._quantity * s._product.Price);
            return (decimal)total;
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