using prueba_salud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prueba_salud.Controllers
{
    //[Route("products/")]
    public class productsController : Controller
    {
        // GET: products

        [HttpGet]
        public ActionResult Index(String message = "")
        {
            using (store_dbEntities db = new store_dbEntities())
            {
                var products = (from p in db.products join c in db.categories on p.category_id equals c.id
                                where p.active == 1
                                select new productsVm { product = p, category = c }).ToList();
                ViewData["products"] = products;
                ViewBag.message = message;
                return View();
            }
        }

        [HttpGet]
        [Route("products/all")]
        public ActionResult all()
        {
            using (store_dbEntities db = new store_dbEntities())
            {
                var prods = (from p in db.products
                             join c in db.categories on p.category_id equals c.id
                             select new { product = p.product1, category = c.categorie }).ToList();

                return Json(prods, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult form(int? id = null)
        {
            ViewBag.categories = getCategories();

            if (id == null)
                return View();

            using (store_dbEntities db = new store_dbEntities())
            {
                var prod = (from p in db.products
                            join c in db.categories on p.category_id equals c.id
                            where p.id == id
                            select new productsVm
                            {
                                id = p.id,
                                product1 = p.product1,
                                description = p.description,
                                price = p.price,
                                date_in = p.date_in,
                                category_id = p.category_id
                            }).FirstOrDefault();
                ViewBag.id = id;
                return View(prod);
            }
        }

        [HttpPost]
        public ActionResult form(productsVm prod)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.categories = getCategories();
                return View(prod);
            }

            using (store_dbEntities db = new store_dbEntities())
            {
                var p = new product();
                if(prod.id != 0)
                    p = db.products.Find(prod.id);

                p.product1 = prod.product1;
                p.description = prod.description;
                p.price = prod.price;
                p.category_id = prod.category_id;
                p.date_in = DateTime.Now;

                if(prod.id == 0)
                    db.products.Add(p);
                else
                    db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                    //db.Entry(p).State = System.Data.Entity.EntityState.Modified;                    
                
                db.SaveChanges();
                return RedirectToAction("index", new { message = "registro procesado exitosamente" });
            }

        }

        public ActionResult delete(int id)
        {
            using (store_dbEntities db = new store_dbEntities())
            {
                var p = db.products.Find(id);
                p.active = 0;
                db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("index", new { message = "registro procesado exitosamente" });
            }
        }

        public List<SelectListItem> getCategories()
        {
            using (store_dbEntities db = new store_dbEntities())
            {
                return db.categories.ToList().ConvertAll(c =>
                {
                    return new SelectListItem()
                    {
                        Text = c.categorie,
                        Value = c.id.ToString(),
                        Selected = false
                    };
                });
            }
        }
    }
}