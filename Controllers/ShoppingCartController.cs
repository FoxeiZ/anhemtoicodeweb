using anhemtoicodeweb.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace anhemtoicodeweb.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        private readonly Model1 database = new Model1();
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
                TempData["PhoneNumber"] = _cus.PhoneCus;
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

        [HttpPost]
        public ActionResult AddToCart()
        {
            int id;
            using (StreamReader reader = new StreamReader(HttpContext.Request.InputStream))
            {
                if (!int.TryParse(reader.ReadLine(), out id))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }

            var _pro = database.Products.SingleOrDefault(s => s.ProductID == id);
            if (_pro == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            if (GetCart().AddProductCart(_pro))
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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

                if (form["AddressDelivery"] == "")
                {
                    if (_user.AddressName == null || _user.AddressName == "")
                    {
                        TempData["Error"] = "Bạn cần phải nhập địa chỉ giao hàng";
                        return RedirectToAction("Index");
                    }
                    form["AddressDelivery"] = _user.AddressName;
                }

                if (form["PhoneNumber"] == "")
                {
                    if (_user.PhoneCus == null || _user.PhoneCus == "")
                    {
                        TempData["Error"] = "Bạn cần phải nhập số điện thoại để liên hệ khi giao hàng";
                        return RedirectToAction("Index");
                    }
                    form["PhoneNumber"] = _user.PhoneCus;
                }

                if (form["CodeCustomer"] == null)
                {
                    form["CodeCustomer"] = _user.IDCus.ToString();
                }

                if (_user.AddressName == null)
                {
                    _user.AddressName = form["AddressDelivery"];
                    database.Entry<Customer>(_user).State = EntityState.Modified;
                }

                OrderPro _order = new OrderPro(); //Bang Hoa Don San pham

                _order.DateOrder = DateTime.Now;
                _order.AddressDelivery = form["AddressDelivery"];
                _order.PhoneNumber = form["PhoneNumber"];
                _order.IDCus = int.Parse(form["CodeCustomer"]);
                _order.State = "Đang xử lý";

                decimal totalDiscount = 0;
                decimal totalPrice = 0;
                decimal totalTax = 0;
                int totalQuantity = 0;

                foreach (var item in cart.Items)
                {
                    var prodTotal = (item._quantity * item._product.Price);
                    var tax = prodTotal * item._product.Tax;
                    var discount = item._product.Discount;
                    if (discount >= 0 && discount <= 1)
                    {
                        discount = prodTotal * discount;
                    }

                    OrderDetail _order_detail = new OrderDetail
                    {
                        IDProduct = item._product.ProductID,
                        IDOrder = _order.ID,

                        Quantity = item._quantity,
                        UnitPrice = item._product.Price,
                        Total = prodTotal - discount + tax,
                        Discount = item._product.Discount,
                        Tax = item._product.Tax,
                    };

                    totalQuantity += item._quantity;
                    totalDiscount += _order_detail.Discount;
                    totalPrice += _order_detail.Total;
                    totalTax += _order_detail.Tax;
                    database.OrderDetails.Add(_order_detail);

                    var _prod = database.Products.Find(item._product.ProductID);
                    _prod.InvQuantity = Math.Max(_prod.InvQuantity - item._quantity, 0);
                    database.Entry(_prod).State = EntityState.Modified;
                }

                _order.TotalMoney = totalPrice;
                _order.TotalTax = totalTax;
                _order.TotalDiscount = totalDiscount;
                _order.TotalAmount = totalQuantity;

                database.OrderProes.Add(_order);
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
                return RedirectToAction("Index", "Home");
            }

            return View(_order);
        }

        public ActionResult EditOrder(int? id)
        {
            if (id == null)
            {
                return View();
            }

            OrderPro _order = database.OrderProes.Where(x => x.ID == id).FirstOrDefault();
            if (_order == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var l = new List<String>()
            {
                "Đang xử lý",
                "Đã xử lý",
                "Đang giao hàng",
                "Đã giao hàng",
                "Đang hủy",
                "Đã hủy",
            };

            ViewBag.State = l.Select(x => new SelectListItem { Text = x, Value = x, Selected = (x == _order.State) }).ToList();
            return View(_order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOrder([Bind(Include = "ID,DateOrder,IDCus,PhoneNumber,AddressDelivery,TotalAmount,TotalMoney,TotalTax,TotalDiscount,State")] OrderPro orderPro)
        {
            if (ModelState.IsValid)
            {
                database.Entry(orderPro).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("CheckOrder", new { id = orderPro.ID });
            }

            var l = new List<String>()
            {
                "Đang xử lý",
                "Đã xử lý",
                "Đang giao hàng",
                "Đã giao hàng",
                "Đang hủy",
                "Đã hủy",
            };

            ViewBag.State = l.Select(x => new SelectListItem { Text = x, Value = x, Selected = (x == orderPro.State) }).ToList();

            return View(orderPro);
        }

        public ActionResult DeleteOrder(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderPro orderPro = database.OrderProes.Find(id);
            if (orderPro == null)
            {
                return HttpNotFound();
            }
            return View(orderPro);
        }

        [HttpPost, ActionName("DeleteOrder")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteOrderConfirmed(int id)
        {
            OrderPro orderPro = database.OrderProes.Find(id);
            database.OrderDetails.Where(x => x.IDOrder == orderPro.ID).ForEach(x => database.OrderDetails.Remove(x));
            database.OrderProes.Remove(orderPro);
            database.SaveChanges();
            return RedirectToAction("Details", "Customers", new { id = orderPro.IDCus });
        }

        public ActionResult CancelOrder(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderPro orderPro = database.OrderProes.Find(id);
            if (orderPro == null)
            {
                return HttpNotFound();
            }
            return View(orderPro);
        }

        [HttpPost, ActionName("CancelOrder")]
        [ValidateAntiForgeryToken]
        public ActionResult CancelOrderConfirmed(int id)
        {
            OrderPro orderPro = database.OrderProes.Find(id);
            orderPro.State = "Đang hủy";
            database.Entry(orderPro).State = EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("CheckOrder", new { id = orderPro.ID });
        }
    }
}