using anhemtoicodeweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace KetNoiDatabase.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        Model1 database = new Model1();
        public ActionResult Index()
        {
            if (Session["Cart"] == null)
            {
                return View();
            }
            Cart cart = Session["Cart"] as Cart;

            int _id = (int?)Session["UserId"] ?? -1;
            if (_id > -1)
            {
                Customer _cus = database.Customers.FirstOrDefault(x => x.IDCus == _id);
                TempData["Address"] = _cus.AddressName;
            }
            return View(cart);
        }

        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        public ActionResult AddToCart(int id)
        {
            var _pro = database.Products.SingleOrDefault(s => s.ProductID == id);
            if (_pro != null)
            {
                GetCart().AddProductCart(_pro);
            }
            return RedirectToAction("Index", "ShoppingCart");
        }

        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.RemoveCartItem(id);
            return RedirectToAction("Index", "ShoppingCart");
        }

        public PartialViewResult BagCart()
        {
            int total_quantity_item = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
                total_quantity_item = cart.TotalQuantity();

            ViewBag.QuantityCart = total_quantity_item;
            return PartialView("BagCart");
        }

        public ActionResult UpdateCartQuantity(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int id_pro = int.Parse(form["idPro"]);
            int _quantity = int.Parse(form["cartQuantity"]);
            cart.UpdateQuantity(id_pro, _quantity);
            return RedirectToAction("Index", "ShoppingCart");
        }

        public ActionResult CheckOut(FormCollection form)
        {
            try
            {
                Cart cart = Session["Cart"] as Cart;
                int _userId = (int)Session["UserId"];
                var _user = database.Customers.FirstOrDefault(x => x.IDCus == _userId);

                if (cart.Items.Count() == 0)
                {
                    return RedirectToAction("Index");
                }

                if (form["AddressDelivery"] == null)
                {
                    if (_user.AddressName == null)
                    {
                        TempData["Error"] = "Bạn cần phải nhập địa chỉ giao hàng";
                        return RedirectToAction("Index");
                    }
                    form["AddressDelivery"] = _user.AddressName;
                }

                if (form["CodeCustomer"] == null)
                {
                    form["CodeCustomer"] = _user.IDCus.ToString();

                }

                if (_user.AddressName == null)
                {
                    _user.AddressName = form["AddressDelivery"];
                    database.Entry<Customer>(_user).State = EntityState.Modified;
                    database.SaveChanges();
                }

                OrderPro _order = new OrderPro(); //Bang Hoa Don San pham

                _order.DateOrder = DateTime.Now;
                _order.AddressDelivery = form["AddressDelivery"];
                _order.IDCus = int.Parse(form["CodeCustomer"]);
                database.OrderProes.Add(_order);
                foreach (var item in cart.Items)
                {
                    OrderDetail _order_detail = new OrderDetail
                    {
                        IDOrder = _order.ID,
                        IDProduct = item._product.ProductID,
                        UnitPrice = (double)item._product.Price,
                        Quantity = item._quantity
                    };
                    database.OrderDetails.Add(_order_detail);
                }

                database.SaveChanges();
                cart.ClearCart();
                return View("CheckOutSuccess", _order);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult BuyNow(int id)
        {
            Cart cart = GetCart();
            var _pro = database.Products.SingleOrDefault(s => s.ProductID == id);
            if (_pro != null)
            {
                cart.AddProductCart(_pro);
                if (Session["UserId"] == null)
                {
                    TempData["Error"] = "Bạn cần phải đăng nhập trước khi thanh toán";
                    return RedirectToAction("Index");
                }
                int _userId = (int)Session["UserId"];
                var _user = database.Customers.FirstOrDefault(x => x.IDCus == _userId);
                FormCollection form = new FormCollection();
                form["AddressDelivery"] = _user.AddressName;
                form["CodeCustomer"] = _user.IDCus.ToString();
                return RedirectToAction("CheckOut", "ShoppingCart", form);
            }
            TempData["Error"] = "Sản phẩm không tồn tại hoặc đã bị xóa";
            return RedirectToAction("Index", "Products");
        }

        public ActionResult CheckOutSuccess(OrderPro _order)
        {
            return View(_order);
        }

        public ActionResult CheckOrder(int? id)
        {
            if (id == null)
            {
                return View();
            }
            OrderPro _order = database.OrderProes.Where(x => x.ID == id).FirstOrDefault();
            if (_order == null)
            {
                return View();
            }
            //List<OrderDetail> order_items = new List<OrderDetail>();

            Product product;
            List<CartItem> cartItems = new List<CartItem>();
            var order_items = database.OrderDetails.Where(x => x.IDOrder == _order.ID).ToList();
            foreach (var item in order_items)
            {
                product = database.Products.Where(x => x.ProductID == item.ID).FirstOrDefault();
                CartItem cartItem = new CartItem { _quantity = (int)item.Quantity, _product = item.Product };
                cartItems.Add(cartItem);
            }
            return View(cartItems);
        }
    }
}