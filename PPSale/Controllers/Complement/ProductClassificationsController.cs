using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PPSale.Classes;
using PPSale.Models.Complement;
using PPSale.Models.Conexion;

namespace PPSale.Controllers.Complement
{
    public class ProductClassificationsController : Controller
    {
        private ConexionContext db = new ConexionContext();
        private FunctionHelpers fn = new FunctionHelpers();

        // GET: ProductClassifications
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

                TempData["Action"] = "Success";
                TempData["Message"] = fn.notRegistre;
            }

            var productClassifications = db.ProductClassifications.
                Include(p => p.Classification).
                Include(p => p.Product).
                Where(p=>p.CompanyId==asing.CompanyId).
                ToList();

            return View(productClassifications);
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
