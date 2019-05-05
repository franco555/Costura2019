using PPSale.Classes;
using PPSale.Models.Conexion;
using PPSale.Models.Entry;
using PPSale.Models.Globals;
using PPSale.Models.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace PPSale.Controllers.Entry
{
    public class DocumentEntriesController : Controller
    {
        private ConexionContext db = new ConexionContext();
        private FunctionHelpers fn = new FunctionHelpers();

        // GET: DocumentEntries
        public ActionResult Index()
        {
            var asing = db.AsingRolAndUsers.Where(a => a.Email == User.Identity.Name).FirstOrDefault();

            if (asing == null)
            {
                asing = new AsingRolAndUser
                {
                    AsingRolAndUserId = 0,
                    CompanyId = 0,
                };

                TempData["Error"] = "Error";
                TempData["Message"] = fn.notRegistre;
            }

            var documentEntries = db.DocumentEntries.
                Include(d => d.CategoryDocument).
                Include(d => d.Provider).
                Include(d => d.Company).
                Include(d => d.TypeDocument).
                Include(d => d.TypePayment).
                Where(d => d.CompanyId == asing.CompanyId).
                OrderByDescending(d => d.Date).
                ToList();

            
            return View(documentEntries);
        }

        // GET: DocumentEntries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            DocumentEntry documentEntry = db.DocumentEntries.Find(id);
            if (documentEntry == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }
            return View(documentEntry);
        }

        // GET: DocumentEntries/Create
        public ActionResult Create()
        {
            var asing = db.AsingRolAndUsers.Where(a => a.Email == User.Identity.Name).FirstOrDefault();
            if (asing==null){ return RedirectToAction("Entry","Navs");}

            var DocumentEntry = new DocumentEntry
            {
                CompanyId = asing.CompanyId,
                Date = DateTime.Now,
            };

            ViewBag.CategoryDocumentId = new SelectList(CombosHelpers.GetCategoryDocuments(0), "CategoryDocumentId", "Name");
            ViewBag.ProviderId = new SelectList(CombosHelpers.GetProviders(asing.CompanyId), "ProviderId", "Name");
            ViewBag.TypeDocumentId = new SelectList(CombosHelpers.GetTypeDocuments(), "TypeDocumentId", "Name");
            ViewBag.TypePaymentId = new SelectList(CombosHelpers.GetTypePayments(), "TypePaymentId", "Name");

            return PartialView(DocumentEntry);
        }

        // POST: DocumentEntries/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DocumentEntry documentEntry)
        {
            if (ModelState.IsValid)
            {
                documentEntry.PriceTotal =0;
                documentEntry.Iva = 0;
                documentEntry.save = false;
                documentEntry.Payment = false;

                db.DocumentEntries.Add(documentEntry);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    //return RedirectToAction("Index");
                    return RedirectToAction("AddProductEntry","DocumentEntries", new { id=documentEntry.DocumentEntryId});
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            ModelState.AddModelError(string.Empty, "Modelo no válido");
            

            ViewBag.CategoryDocumentId = new SelectList(CombosHelpers.GetCategoryDocuments(documentEntry.TypeDocumentId), "CategoryDocumentId", "Name", documentEntry.CategoryDocumentId);
            ViewBag.ProviderId = new SelectList(CombosHelpers.GetProviders(documentEntry.CompanyId), "ProviderId", "Name", documentEntry.ProviderId);
            ViewBag.TypeDocumentId = new SelectList(CombosHelpers.GetTypeDocuments(), "TypeDocumentId", "Name", documentEntry.TypeDocumentId);
            ViewBag.TypePaymentId = new SelectList(CombosHelpers.GetTypePayments(), "TypePaymentId", "Name", documentEntry.TypeDocumentId);

            return View(documentEntry);
        }

        // GET: DocumentEntries/Edit/5
        public ActionResult Edit(int? id)
        {
            var asing = db.AsingRolAndUsers.Where(a => a.Email == User.Identity.Name).FirstOrDefault();
            if (asing == null) { return RedirectToAction("Index"); }

            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            DocumentEntry documentEntry = db.DocumentEntries.Find(id);
            if (documentEntry == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }

            ViewBag.CategoryDocumentId = new SelectList(CombosHelpers.GetCategoryDocuments(documentEntry.TypeDocumentId), "CategoryDocumentId", "Name", documentEntry.CategoryDocumentId);
            ViewBag.ProviderId = new SelectList(CombosHelpers.GetProviders(documentEntry.CompanyId), "ProviderId", "Name", documentEntry.ProviderId);
            ViewBag.TypeDocumentId = new SelectList(CombosHelpers.GetTypeDocuments(), "TypeDocumentId", "Name", documentEntry.TypeDocumentId);
            ViewBag.TypePaymentId = new SelectList(CombosHelpers.GetTypePayments(), "TypePaymentId", "Name", documentEntry.TypeDocumentId);

            return View(documentEntry);
        }

        // POST: DocumentEntries/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( DocumentEntry documentEntry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(documentEntry).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            ModelState.AddModelError(string.Empty, "Modelo no válido");
            

            ViewBag.CategoryDocumentId = new SelectList(CombosHelpers.GetCategoryDocuments(documentEntry.TypeDocumentId), "CategoryDocumentId", "Name", documentEntry.CategoryDocumentId);
            ViewBag.ProviderId = new SelectList(CombosHelpers.GetProviders(documentEntry.CompanyId), "ProviderId", "Name", documentEntry.ProviderId);
            ViewBag.TypeDocumentId = new SelectList(CombosHelpers.GetTypeDocuments(), "TypeDocumentId", "Name", documentEntry.TypeDocumentId);
            ViewBag.TypePaymentId = new SelectList(CombosHelpers.GetTypePayments(), "TypePaymentId", "Name", documentEntry.TypePaymentId);

            return View(documentEntry);
        }

        // GET: DocumentEntries/Delete/5
        public ActionResult Delete(int? id)
        {/*
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            DocumentEntry documentEntry = db.DocumentEntries.Find(id);
            if (documentEntry == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }
            return View(documentEntry);*/
            return View();
        }

        // POST: DocumentEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DocumentEntry documentEntry = db.DocumentEntries.Find(id);
            db.DocumentEntries.Remove(documentEntry);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: Proforma
        public ActionResult AddProductEntry(int? id) {
           
            if (id==null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Entry", "Navs");
            }

            var asing = db.AsingRolAndUsers.Where(a => a.Email == User.Identity.Name).FirstOrDefault();

            if (asing == null)
            {
                asing = new AsingRolAndUser
                {
                    AsingRolAndUserId = 0,
                    CompanyId = 0,
                };

                TempData["Error"] = "Necesita que el usuario este registado en alguna empresa, Éste usuario no tiene permiso de Modificar esta opción. Por favor comuniquese con el administrador";
                TempData["Valid"] = true;
            }


            var documentEntry = db.DocumentEntries.Find(id);
            //var documentEntry = db.DocumentEntries.Where(d => d.DocumentEntryId == id && d.CompanyId == asing.CompanyId).ToList();
            if (documentEntry.CompanyId != asing.CompanyId)
            {
                TempData["Valid"] = true;
                TempData["Error"] = "No existe Registro Con esta compañia y Documento de Entrada";
                return RedirectToAction("Entry", "Navs");
            }
            
            var s = new AddProductEntries {
                DocumentEntryId=documentEntry.DocumentEntryId,
                Date = documentEntry.Date,
                TypeDocument = documentEntry.TypeDocument.Name,
                CategoryDocument = documentEntry.CategoryDocument.Name,
                NumberDoc = documentEntry.NumberDoc,
                PriceTotal = documentEntry.PriceTotal,
                P_Total = GetPrice(documentEntry.DocumentEntryId),
                Iva = documentEntry.Iva,
                IvaTotal= documentEntry.Iva * GetPrice(documentEntry.DocumentEntryId),
                P_Total_Iva= GetPrice(documentEntry.DocumentEntryId)+ (documentEntry.Iva * GetPrice(documentEntry.DocumentEntryId)),

                p_Provider = documentEntry.Provider.Name,
                p_Address = documentEntry.Provider.Address,
                p_Cuit = documentEntry.Provider.Cuit,
                p_Email = documentEntry.Provider.Email,
                p_Logo = documentEntry.Provider.Logo.Substring(2),
                p_IVA = documentEntry.Provider.IvaCondition.Name,
                p_Date = documentEntry.Provider.FirstDate.ToString("dd/MM/yyyy"),

                c_Company = documentEntry.Company.Name,
                c_Address = documentEntry.Company.Address,
                c_Cuit =documentEntry.Company.Cuit,
                c_Email=documentEntry.Company.Email,
                c_Logo =documentEntry.Company.Logo.Substring(2),
                c_IVA = documentEntry.Company.IvaCondition.Name,
                c_Date = documentEntry.Company.FirstDate.ToString("dd/MM/yyyy"),

                TypePayment = documentEntry.TypePayment.Name,
                save = documentEntry.save,
                Payment = documentEntry.Payment,
                DocumentEntryDetails = GetDocumentDetails(id),
            }; 
            return View(s);
        }

        public double GetPrice(int idDocumentEntry)
        {
            
            var Stock = (from ded in db.DocumentEntryDetails
                         where ded.DocumentEntryId ==idDocumentEntry
                         group ded by new { ded.DocumentEntryId } into stock
                         select new
                         {
                             Price = stock.Sum(x => x.Price * x.Quantity),
                         })
            .ToList();

            double price = 0;
            if (Stock.Count>0)
            {
                price = Stock[0].Price;
            }

            return price;
        }

        public static List<DocumentEntryDetail> GetDocumentDetails(int? documetnEntryId)
        {
            var db1 = new ConexionContext();
            var detail = db1.DocumentEntryDetails.Where(w => w.DocumentEntryId == documetnEntryId).ToList();

            var detailsMod = new List<DocumentEntryDetail>();

            foreach (var item in detail)
            {
                detailsMod.Add(new DocumentEntryDetail() {
                    DocumentEntryDetailId = item.DocumentEntryDetailId,
                    ProductClassificationId=item.ProductClassificationId,
                    ProductId = item.ProductClassification.ProductId,
                    ClassificationId = item.ProductClassification.ClassificationId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    MeasuredUnitId = item.MeasuredUnitId,
                    DocumentEntryId = item.DocumentEntryId,
                    CompanyId = item.CompanyId,
                    save = item.save,
                    FullName = "Prueba en modelo",//GetProduct(item.ProductId, item.ClassificationId),

                    OtherProduct = GetProduct(1, 1),
                });
            }
            return detailsMod;
        }

        public static string GetProduct(int ProductId, int ClassificationId)
        {
            var db1 = new ConexionContext();
            var p = db1.Products.Find(ProductId);
            var c = db1.Classifications.Find(ClassificationId);
            db1.Dispose();

            return string.Format("{0} {1}", p.Name, c.Name);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DocumentEntrySave(int Id)
        {
            
            return RedirectToAction("","",new { id=Id});
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
