﻿using anhemtoicodeweb.Enums;
using anhemtoicodeweb.Library;
using anhemtoicodeweb.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Configuration;
using System.Data.Entity;
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
                User _cus = database.Users.FirstOrDefault(x => x.ID == _id);
                TempData["Address"] = _cus.AddressName;
                TempData["PhoneNumber"] = _cus.Phone;
            }

            ViewBag.PaymentMethod = Enums.PaymentMethodEnum.AllPaymentMethod.Select(
                s => new SelectListItem
                {
                    Text = s.Value,
                    Value = s.Name,
                    Selected = s.Equals(Enums.PaymentMethodEnum.Cash)
                }).ToList();

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
                var _user = database.Users.FirstOrDefault(x => x.ID == _userId);

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
                    if (_user.Phone == null || _user.Phone == "")
                    {
                        TempData["Error"] = "Bạn cần phải nhập số điện thoại để liên hệ khi giao hàng";
                        return RedirectToAction("Index");
                    }
                    form["PhoneNumber"] = _user.Phone;
                }

                if (form["CodeCustomer"] == null)
                {
                    form["CodeCustomer"] = _user.ID.ToString();
                }

                var paymentMethod = form["PaymentMethod"];
                if (Enums.PaymentMethodEnum.FindByName(paymentMethod) == null)
                {
                    TempData["CheckoutError"] = "Chức năng thanh toán không hợp lệ.";
                    return RedirectToAction("Index");
                }

                if (_user.AddressName == null)
                {
                    _user.AddressName = form["AddressDelivery"];
                    database.Entry<User>(_user).State = EntityState.Modified;
                }

                //OrderPro _order = new OrderPro(); //Bang Hoa Don San pham

                //_order.DateOrder = DateTime.Now;
                //_order.AddressDelivery = form["AddressDelivery"];
                //_order.PhoneNumber = form["PhoneNumber"];
                //_order.IDCus = int.Parse(form["CodeCustomer"]);
                //_order.State = Enums.OrderState.Pending.Name;

                var _order = new OrderPro
                {
                    //ID = GenerateRandomOrderId(), // Randomly generated OrderID
                    IDCus = _user.ID,
                    State = Enums.OrderState.Pending.Name,
                    CreatedAt = DateTime.Now,
                    PaymentMethod = paymentMethod
                };

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
                    _prod.Quantity = Math.Max(_prod.Quantity - item._quantity, 0);
                    database.Entry(_prod).State = EntityState.Modified;
                }

                _order.TotalMoney = totalPrice;
                _order.TotalTax = totalTax;
                _order.TotalDiscount = totalDiscount;
                _order.TotalAmount = totalQuantity;

                database.OrderProes.Add(_order);
                database.SaveChanges();

                if (paymentMethod.ToLower() == "vnpay")
                {
                    return Redirect(GenerateVNPayUrl(_order));
                }

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
                var _user = database.Users.FirstOrDefault(x => x.ID == _userId);
                FormCollection form = new FormCollection();
                form["AddressDelivery"] = _user.AddressName;
                form["CodeCustomer"] = _user.ID.ToString();
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

            ViewBag.State = Enums.OrderState.AllState.Select(x => new SelectListItem { Text = x.Value, Value = x.Name, Selected = (x.Value == _order.State) }).ToList();
            return View(_order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOrder([Bind(Include = "ID,DateOrder,ID,PhoneNumber,AddressDelivery,TotalAmount,TotalMoney,TotalTax,TotalDiscount,State,CreatedAt")] OrderPro orderPro, string oldState)
        {
            //var oldState = database.OrderProes.Find(orderPro.ID);
            if (ModelState.IsValid && OrderState.CanMoveToState(oldState, orderPro.State))
            {
                database.Entry(orderPro).State = EntityState.Modified;
                //database.OrderProes.AddOrUpdate(orderPro);
                database.SaveChanges();
                return RedirectToAction("CheckOrder", new { id = orderPro.ID });
            }

            ViewBag.State = OrderState.AllState.Select(x => new SelectListItem { Text = x.Value, Value = x.Name, Selected = (x.Value == orderPro.State) }).ToList();

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
            return RedirectToAction("Details", "Users", new { id = orderPro.IDCus });
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
            orderPro.State = OrderState.Canceled.Name;
            database.Entry(orderPro).State = EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("CheckOrder", new { id = orderPro.ID });
        }

        // Generate a unique random OrderID
        private int GenerateRandomOrderId()
        {
            Random random = new Random();
            int orderId;
            do
            {
                orderId = random.Next(100000, 999999); // 6-digit random number
            } while (database.OrderProes.Any(o => o.ID == orderId)); // Ensure uniqueness
            return orderId;
        }

        private string GenerateVNPayUrl(OrderPro order)
        {
            string vnpUrl = ConfigurationManager.AppSettings["vnp_Url"];
            string vnpTmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"];
            string vnpHashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"];
            string vnpReturnUrl = Url.Action("VNPayReturn", "ShoppingCart", null, Request.Url.Scheme);
            string ipAddr = Utils.GetIpAddress();

            var vnpay = new VNPay();
            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnpTmnCode);
            vnpay.AddRequestData("vnp_Amount", ((int)(order.TotalAmount * 100)).ToString());
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_TxnRef", order.ID.ToString());
            vnpay.AddRequestData("vnp_OrderInfo", $"Payment for Order #{order.ID}");
            vnpay.AddRequestData("vnp_OrderType", "billpayment");
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_ReturnUrl", vnpReturnUrl);
            vnpay.AddRequestData("vnp_IpAddr", ipAddr);

            string createDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            vnpay.AddRequestData("vnp_CreateDate", createDate);

            return vnpay.CreateRequestUrl(vnpUrl, vnpHashSecret);
        }

        // Handle VNPay return
        public ActionResult VNPayReturn()
        {
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"];
            var vnpay = new VNPay();

            foreach (string key in Request.QueryString.Keys)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    vnpay.AddResponseData(key, Request.QueryString[key]);
                }
            }

            string vnpSecureHash = Request.QueryString["vnp_SecureHash"];
            bool isValidSignature = vnpay.ValidateSignature(vnpSecureHash, vnp_HashSecret);

            if (isValidSignature)
            {
                string responseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string transactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                string txnRef = vnpay.GetResponseData("vnp_TxnRef");

                if (responseCode == "00" && transactionStatus == "00")
                {
                    if (int.TryParse(txnRef, out int orderId))
                    {
                        var order = database.OrderProes.Find(orderId);
                        if (order != null)
                        {
                            order.State = Enums.OrderState.PaymentReceived.Name;
                            database.SaveChanges();
                            Session["Cart"] = null;
                            Session["CartCount"] = 0;
                            return RedirectToAction("OrderConfirmation", new { orderId });
                        }
                    }
                    TempData["CheckoutError"] = "Invalid order reference.";
                }
                else
                {
                    TempData["CheckoutError"] = "Thanh toán bị huỷ. Vui lòng thử lại sau.";
                }
            }
            else
            {
                TempData["CheckoutError"] = "Invalid payment response.";
            }
            return RedirectToAction("Index");
        }
    }
}