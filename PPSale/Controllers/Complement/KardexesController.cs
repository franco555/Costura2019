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
using PPSale.Models.View;

namespace PPSale.Controllers.Complement
{
    public class KardexesController : Controller
    {
        private ConexionContext db = new ConexionContext();

        // GET: Kardexes
        public ActionResult Index()
        {
            TempData["Valid"] = null;
            TempData["Error"] = null;

            var asing = db.AsingRolAndUsers.Where(a => a.Email == User.Identity.Name).FirstOrDefault();

            if (asing == null)
            {
                asing = new Models.Globals.AsingRolAndUser
                {
                    AsingRolAndUserId = 0,
                    CompanyId = 0,
                };

                TempData["Error"] = "Necesita que el usuario este registado en alguna empresa, Éste usuario no tiene permiso de Modificar esta opción. Por favor comuniquese con el administrador";
                TempData["Valid"] = true;
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
