using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Products
        public ActionResult Index()
        {
            var items = db.Products.ToList();
            
            return View(items);
        }

        public ActionResult Detail(string alias,int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                db.Products.Attach(item);
                item.ViewCount = item.ViewCount + 1;
                db.Entry(item).Property(x => x.ViewCount).IsModified = true;
                db.SaveChanges();
            }
            
            return View(item);
        }
        public ActionResult ProductCategory(string alias,int id)
        {
            var items = db.Products.ToList();
            if (id > 0)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
            }
            var cate = db.ProductCategories.Find(id);
            if (cate != null)
            {
                ViewBag.CateName = cate.Title;
            }

            ViewBag.CateId = id;
            return View(items);
        }

        public ActionResult Partial_ItemsByCateId()
        {
            var items = db.Products.Where(x => x.IsHome && x.IsActive).Take(12).ToList();
            return PartialView(items);
        }

        public ActionResult Partial_ProductSales()
        {
            var items = db.Products.Where(x => x.IsSale && x.IsActive).Take(12).ToList();
            return PartialView(items);
        }
        [HttpPost]
        public ActionResult DecreaseProductQuantity(int id)
        {
            // Load product from database
            var product = db.Products.Find(id);

            // Decrease quantity
            if (product.Quantity > 0)
            {
                product.Quantity -= 1;
                db.SaveChanges();

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Product is out of stock" });
            }
        }
    }
}