using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PPSale.Classes;
using PPSale.Models.Conexion;
using PPSale.Models.Entry;
using PPSale.Models.Globals;

namespace PPSale.Controllers.Entry
{
    public class DocumentEntryDetailsController : Controller
    {
        private ConexionContext db = new ConexionContext();

        // GET: DocumentEntryDetails
        public ActionResult Index()
        {
            var documentEntryDetails = db.DocumentEntryDetails.
                Include(d => d.ProductClassification).
                Include(d => d.DocumentEntry).
                Include(d => d.MeasuredUnit);

            return View(documentEntryDetails.ToList());
        }

        // GET: DocumentEntryDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentEntryDetail documentEntryDetail = db.DocumentEntryDetails.Find(id);
            if (documentEntryDetail == null)
            {
                return HttpNotFound();
            }
            return View(documentEntryDetail);
        }

        // GET: DocumentEntryDetails/Create
        public ActionResult Create()
        {
            ViewBag.ClassificationId = new SelectList(db.Classifications, "ClassificationId", "Name");
            ViewBag.DocumentEntryId = new SelectList(db.DocumentEntries, "DocumentEntryId", "NumberDoc");
            ViewBag.MeasuredUnitId = new SelectList(db.MeasuredUnits, "MeasuredUnitId", "Name");
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name");

            return View();
        }

        // POST: DocumentEntryDetails/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DocumentEntryDetail documentEntryDetail)
        {
            if (ModelState.IsValid)
            {
                db.DocumentEntryDetails.Add(documentEntryDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClassificationId = new SelectList(db.Classifications, "ClassificationId", "Name", documentEntryDetail.ClassificationId);
            ViewBag.DocumentEntryId = new SelectList(db.DocumentEntries, "DocumentEntryId", "NumberDoc", documentEntryDetail.DocumentEntryId);
            ViewBag.MeasuredUnitId = new SelectList(db.MeasuredUnits, "MeasuredUnitId", "Name", documentEntryDetail.MeasuredUnitId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", documentEntryDetail.ProductId);
            return View(documentEntryDetail);
        }

        // GET: DocumentEntryDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var documentEntryDetail = db.DocumentEntryDetails.Find(id);
            if (documentEntryDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassificationId = new SelectList(db.Classifications, "ClassificationId", "Name", documentEntryDetail.ClassificationId);
            ViewBag.DocumentEntryId = new SelectList(db.DocumentEntries, "DocumentEntryId", "NumberDoc", documentEntryDetail.DocumentEntryId);
            ViewBag.MeasuredUnitId = new SelectList(db.MeasuredUnits, "MeasuredUnitId", "Name", documentEntryDetail.MeasuredUnitId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", documentEntryDetail.ProductId);
            return View(documentEntryDetail);
        }

        // POST: DocumentEntryDetails/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DocumentEntryDetail documentEntryDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(documentEntryDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassificationId = new SelectList(db.Classifications, "ClassificationId", "Name", documentEntryDetail.ClassificationId);
            ViewBag.DocumentEntryId = new SelectList(db.DocumentEntries, "DocumentEntryId", "NumberDoc", documentEntryDetail.DocumentEntryId);
            ViewBag.MeasuredUnitId = new SelectList(db.MeasuredUnits, "MeasuredUnitId", "Name", documentEntryDetail.MeasuredUnitId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", documentEntryDetail.ProductId);
            return View(documentEntryDetail);
        }

        // GET: DocumentEntryDetails/Delete/5
        /*public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentEntryDetail documentEntryDetail = db.DocumentEntryDetails.Find(id);
            if (documentEntryDetail == null)
            {
                return HttpNotFound();
            }
            return View(documentEntryDetail);
        }

        // POST: DocumentEntryDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DocumentEntryDetail documentEntryDetail = db.DocumentEntryDetails.Find(id);
            db.DocumentEntryDetails.Remove(documentEntryDetail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }*/


        //Ventanas modales para agregar productos

        public ActionResult AddProductEntryNew(int id)
        {
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

                return RedirectToAction("Entry", "Navs");
            }


            var documentEntryDetail = new DocumentEntryDetail
            {
                DocumentEntryId = id,
                CompanyId = asing.CompanyId,
            };

            ViewBag.ClassificationId = new SelectList(CombosHelpers.GetClassifications(0, asing.CompanyId), "ClassificationId", "Name");
            ViewBag.MeasuredUnitId = new SelectList(CombosHelpers.GetMeasuredUnit(0), "MeasuredUnitId", "Name");
            ViewBag.ProductId = new SelectList(CombosHelpers.GetProducts(asing.CompanyId), "ProductId", "Name");

            return PartialView(documentEntryDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProductEntryNewSave(DocumentEntryDetail documentEntryDetail)
        {
            var asing = db.AsingRolAndUsers.Where(a => a.Email == User.Identity.Name).FirstOrDefault();
            if (asing == null)
            {
                TempData["Error"] = "Necesita que el usuario este registado en alguna empresa, Éste usuario no tiene permiso de Modificar esta opción [AddProductEntryNewSave]. Por favor comuniquese con el administrador";
                return RedirectToAction("AddProductEntry", "DocumentEntries", new { id = documentEntryDetail.DocumentEntryId });
            }
            

            documentEntryDetail.CompanyId = asing.CompanyId;
            documentEntryDetail.save = false;
            
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var IdPc = GetProductClassificationId(documentEntryDetail.ProductId, documentEntryDetail.ClassificationId, documentEntryDetail.CompanyId);
                        var db2 = new ConexionContext();
                        var TempDetail = db2.DocumentEntryDetails.Where(t => t.DocumentEntryId == documentEntryDetail.DocumentEntryId && t.ProductClassificationId == IdPc).FirstOrDefault();
                        db2.Dispose();

                        documentEntryDetail.ProductClassificationId = IdPc;

                        if (TempDetail == null)
                        {
                            documentEntryDetail.edit = false;
                            db.DocumentEntryDetails.Add(documentEntryDetail);

                            var newTemp = new TempDocEntryDetil {
                                TempDocEntryDetilId = 0,
                                DocumentEntryDetailId=documentEntryDetail.DocumentEntryDetailId,
                                ProductClassificationId =documentEntryDetail.ProductClassificationId,
                                MeasuredUnitId=documentEntryDetail.MeasuredUnitId,
                                Quantity=documentEntryDetail.Quantity,
                                Price=documentEntryDetail.Price,
                                State=false,
                                Date=DateTime.Now,
                            };

                            db.TempDocEntryDetils.Add(newTemp);
                            //throw new ArgumentException("A start-up parameter is required.");
                        }
                        else
                        {
                            documentEntryDetail.Quantity += TempDetail.Quantity;
                            documentEntryDetail.edit = true;
                            
                            db.Entry(documentEntryDetail).State = EntityState.Modified;

                            var newTemp = new TempDocEntryDetil
                            {
                                TempDocEntryDetilId = 0,
                                DocumentEntryDetailId = documentEntryDetail.DocumentEntryDetailId,
                                ProductClassificationId = documentEntryDetail.ProductClassificationId,
                                MeasuredUnitId = documentEntryDetail.MeasuredUnitId,
                                Quantity = documentEntryDetail.Quantity,
                                Price = documentEntryDetail.Price,
                                State = false,
                                Date = DateTime.Now,
                            };

                            db.TempDocEntryDetils.Add(newTemp);
                        }

                        var docEntry = db.DocumentEntries.Find(documentEntryDetail.DocumentEntryId);
                        docEntry.save = false;
                        db.Entry(docEntry).State = EntityState.Modified;

                        db.SaveChanges();
                        transaction.Commit();

                        TempData["Error"] = null;
                        return RedirectToAction("AddProductEntry", "DocumentEntries", new { id = documentEntryDetail.DocumentEntryId });
                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        TempData["Error"] = ex.Message;

                        return RedirectToAction("AddProductEntry", "DocumentEntries", new { id = documentEntryDetail.DocumentEntryId });
                    }
                }
            }
            else
            {
                TempData["Error"] = "Modelo No valido en AddProductEntryNewSave, Puede ser el uso de como o punto En el precio. De lo COntrario Comuniquese con el administrador";
                TempData["Valid"] = true;
            }
            return RedirectToAction("AddProductEntry", "DocumentEntries", new { id = documentEntryDetail.DocumentEntryId });
        }

        public int  GetProductClassificationId(int IdProduct,int IdClassification, int IdCompany)
        {
            
            var productClassificationId = db.ProductClassifications.Where(
                pc => pc.ProductId == IdProduct &&
                pc.ClassificationId == IdClassification &&
                pc.CompanyId == IdCompany).FirstOrDefault();

            return productClassificationId.ProductClassificationId;
        }


        public ActionResult AddProductEntryEdit(int id)
        {
            var asing = db.AsingRolAndUsers.Where(a => a.Email == User.Identity.Name).FirstOrDefault();
            if (asing == null) { return RedirectToAction("Entry", "Navs"); }

            var documentEntryDetail = db.DocumentEntryDetails.Find(id);

            if (documentEntryDetail == null || documentEntryDetail.CompanyId != asing.CompanyId)
            {
                TempData["Error"] = "No se encontró registros, o No pertenece a la compañía";
                TempData["Valid"] = true;
                return RedirectToAction("AddProductEntry", "DocumentEntries", new { id = documentEntryDetail.DocumentEntryId });
            }

            ViewBag.ProductId = new SelectList(CombosHelpers.GetProducts(asing.CompanyId), "ProductId", "Name", documentEntryDetail.ProductId);
            ViewBag.ClassificationId = new SelectList(CombosHelpers.GetClassifications(documentEntryDetail.ProductId, documentEntryDetail.CompanyId), "ClassificationId", "Name", documentEntryDetail.ClassificationId);
            ViewBag.MeasuredUnitId = new SelectList(CombosHelpers.GetMeasuredUnit(documentEntryDetail.CompanyId), "MeasuredUnitId", "Name", documentEntryDetail.MeasuredUnitId);

            return PartialView(documentEntryDetail);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProductEntryEditSave(DocumentEntryDetail documentEntryDetail)
        {
            var asing = db.AsingRolAndUsers.Where(a => a.Email == User.Identity.Name).FirstOrDefault();
            if (asing == null)
            {
                TempData["Error"] = "Necesita que el usuario este registado en alguna empresa, Éste usuario no tiene permiso de Modificar esta opción [AddProductEntryNewSave]. Por favor comuniquese con el administrador";
                return RedirectToAction("AddProductEntry", "DocumentEntries", new { id = documentEntryDetail.DocumentEntryId });
            }
            /*
            var db2 = new ConexionContext();
            var TempOldDetail = db2.DocumentEntryDetails.Find(documentEntryDetail.DocumentEntryDetailId);
            db2.Dispose();

            if (TempOldDetail == null)
            {
                TempData["Error"] = "El registro no existe para ser editado! [DocumentEntryDetail][AddProductEntryEditSave]";
                return RedirectToAction("AddProductEntry", "DocumentEntries", new { id = documentEntryDetail.DocumentEntryId });
            }

            if (TempOldDetail.SaveTable == true)
            {
                TempData["Error"] = "Primero Guarda El documento Para prosegir. Si el problema persiste comuniquese con el Administrador";
                return RedirectToAction("AddProductEntry", "DocumentEntries", new { id = documentEntryDetail.DocumentEntryId });
            }

            documentEntryDetail.save = TempOldDetail.save;
            documentEntryDetail.Edit = true;
            documentEntryDetail.OtherTable = TempOldDetail.OtherTable;
            documentEntryDetail.SaveTable = TempOldDetail.SaveTable;

            documentEntryDetail.alt_ClassificationId = TempOldDetail.alt_ClassificationId;
            documentEntryDetail.alt_ProductId = TempOldDetail.alt_ProductId;
            documentEntryDetail.alt_Quantity = TempOldDetail.alt_Quantity;

            if (ModelState.IsValid)
            {

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Si el registro que entra es el mismo 
                        var RegEquals = (documentEntryDetail.ProductId == TempOldDetail.ProductId && documentEntryDetail.ClassificationId == TempOldDetail.ClassificationId) ? true : false;

                        if (RegEquals)
                        {
                            documentEntryDetail.OtherTable = false;
                            documentEntryDetail.SaveTable = false;
                            db.Entry(documentEntryDetail).State = EntityState.Modified;

                            if (TempOldDetail.save == true) // guardado
                            {
                                if (TempOldDetail.Edit == true) //Editado
                                {
                                    var NewEntryDetail = db.DocumentEntryDetails.Where(n =>
                                    n.DocumentEntryId == documentEntryDetail.DocumentEntryId &&
                                    n.ProductId == documentEntryDetail.ProductId &&
                                    n.ClassificationId == documentEntryDetail.ClassificationId).FirstOrDefault();

                                    if (NewEntryDetail.save == true)
                                    {

                                    }
                                }
                                else //No Editado
                                {

                                }
                            }
                            else
                            {

                            }



                        }
                        else // No guardado
                        {

                        }

                        //*****
                        if (documentEntryDetail.ProductId == TempOldDetail.ProductId && documentEntryDetail.ClassificationId == TempOldDetail.ClassificationId)
                        {
                            documentEntryDetail.OtherTable = false;
                            documentEntryDetail.SaveTable = false;
                            db.Entry(documentEntryDetail).State = EntityState.Modified;
                        }
                        else
                        {
                            var NewEntryDetail = db.DocumentEntryDetails.Where(n =>
                                n.DocumentEntryId == documentEntryDetail.DocumentEntryId &&
                                n.ProductId == documentEntryDetail.ProductId &&
                                n.ClassificationId == documentEntryDetail.ClassificationId).FirstOrDefault();

                            if (NewEntryDetail == null)
                            {
                                documentEntryDetail.alt_ProductId = TempOldDetail.ProductId;
                                documentEntryDetail.alt_ClassificationId = TempOldDetail.ClassificationId;
                                documentEntryDetail.alt_Quantity = TempOldDetail.Quantity;
                                documentEntryDetail.save = false;
                                documentEntryDetail.OtherTable = true;
                                documentEntryDetail.SaveTable = (TempOldDetail.save == true) ? true : false;

                                db.Entry(documentEntryDetail).State = EntityState.Modified;
                            }
                            else
                            {
                                if (TempOldDetail.save == true)
                                {
                                    NewEntryDetail.OtherTable = true;
                                    NewEntryDetail.SaveTable = (TempOldDetail.save == true) ? true : false;
                                }
                                NewEntryDetail.alt_ProductId = TempOldDetail.ProductId;
                                NewEntryDetail.alt_ClassificationId = TempOldDetail.ClassificationId;
                                NewEntryDetail.alt_Quantity = TempOldDetail.Quantity;

                                NewEntryDetail.Quantity += documentEntryDetail.Quantity;

                                NewEntryDetail.Edit = true;
                                db.Entry(NewEntryDetail).State = EntityState.Modified;

                                var delete = db.DocumentEntryDetails.Find(documentEntryDetail.DocumentEntryDetailId);
                                db.DocumentEntryDetails.Remove(delete);
                            }

                        }



                        var docEntry = db.DocumentEntries.Find(documentEntryDetail.DocumentEntryId);
                        docEntry.save = false;
                        db.Entry(docEntry).State = EntityState.Modified;

                        db.SaveChanges();
                        transaction.Commit();

                        TempData["Error"] = null;
                        return RedirectToAction("AddProductEntry", "DocumentEntries", new { id = documentEntryDetail.DocumentEntryId });
                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        TempData["Error"] = ex.Message;

                        return RedirectToAction("AddProductEntry", "DocumentEntries", new { id = documentEntryDetail.DocumentEntryId });
                    }
                }
            }
            else
            {
                TempData["Error"] = "Modelo No valido en AddProductEntryNewSave, Puede ser el uso de como o punto En el precio. De lo COntrario Comuniquese con el administrador";
                TempData["Valid"] = true;
            }*/
            return RedirectToAction("AddProductEntry", "DocumentEntries", new { id = documentEntryDetail.DocumentEntryId });
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
