using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PPSale.Classes;
using PPSale.Models.Complement;
using PPSale.Models.Conexion;
using PPSale.Models.View;

namespace PPSale.Controllers.Complement
{
    public class KardexesController : Controller
    {
        private ConexionContext db = new ConexionContext();
        private FunctionHelpers fn = new FunctionHelpers();

        // GET: Kardexes
        public ActionResult Index()
        {
            var asing = db.AsingRolAndUsers.Where(a => a.Email == User.Identity.Name).FirstOrDefault();

            if (asing == null)
            {
                asing = new Models.Globals.AsingRolAndUser
                {
                    AsingRolAndUserId = 0,
                    CompanyId = 0,
                };

                TempData["Error"] = "Error";
                TempData["Message"] = fn.notRegistre;

                return RedirectToAction("Index");
            }

            var query = (from k in db.Kardexes
                         join pc in db.ProductClassifications on k.ProductClassificationId equals pc.ProductClassificationId
                         join p in db.Products on pc.ProductId equals p.ProductId
                         join c in db.Classifications on pc.ClassificationId equals c.ClassificationId
                         join u in db.UnitBases on pc.UnitBaseId equals u.UnitBaseId
                         orderby pc.ProductId 

                         select new { k,pc }).ToList();

            var kardex = new List<KardexViewModel>();
            foreach (var item in query) {
                kardex.Add(new KardexViewModel() {
                    KardexId=item.k.KardexId,
                    ProductClassificationId= item.k.ProductClassificationId,
                    UnitBaseId= item.pc.UnitBaseId,
                    ProductName= item.pc.Product.Name,
                    ClassificationName= item.pc.Classification.Name,
                    UnitBasenName= item.pc.UnitBase.Name,
                    Stock= item.k.Stock,
                    Price= item.k.Price,
                });
            }

            return View(kardex);
        }


        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "Error";
                TempData["Message"] = fn.notId;

                return RedirectToAction("Index");
            }
            var kardex = db.Kardexes.Find(id);
            if (kardex == null)
            {
                TempData["Error"] = "Error";
                TempData["Message"] = fn.notExist;

                return RedirectToAction("Index");
            }

            return PartialView(kardex);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Kardex kardex)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kardex).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    TempData["Action"] = "Success";
                    TempData["Message"] = fn.SuccessUpdate;

                    return RedirectToAction("Index");
                }

                TempData["Action"] = "Error";
                TempData["Message"] = response.Message;
            }
            else
            {
                string messages = JsonConvert.SerializeObject(ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage));

                TempData["Action"] = "Object";
                TempData["Message"] = messages;
            }

            return RedirectToAction("index");
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
