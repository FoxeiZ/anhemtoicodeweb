using anhemtoicodeweb.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;


namespace anhemtoicodeweb.Controllers
{
    public class OrderProesController : Controller
    {
        private Model1 db = new Model1();

        [Filters.RequireLoginWithRole(Enums.Role.Seller, Enums.Role.Admin)]
        public ActionResult List(string filter)
        {

            var orderProes = db.OrderProes.Include(o => o.Customer);
            var enums = Enums.OrderState.FindByName(filter);
            if (enums == null)
            {
                return RedirectToAction("List", new { filter = Enums.OrderState.Pending.Name });
            }

            if (filter != null)
            {
                orderProes = orderProes.Where(o => o.State == filter);
            }

            if (Enums.RoleExtensions.EqualsTo(Session["UserRole"], Enums.Role.Seller))
            {
                int? userId = (int?)Session["UserId"];
                orderProes = orderProes.Where(o => o.OrderDetails.Any(od => od.Product.SellerId == userId));
            }
            ViewBag.Title = enums.Value;
            ViewBag.EnumStates = Enums.OrderState.AllState.Select(
                s => new SelectListItem
                {
                    Text = s.Value,
                    Value = s.Name,
                    Selected = s.Equals(enums)
                }).ToList();
            return View(orderProes);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
