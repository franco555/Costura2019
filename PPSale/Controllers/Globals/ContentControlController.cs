namespace PPSale.Controllers.Globals
{
    using Newtonsoft.Json;
    using PPSale.Classes;
    using PPSale.Models.Conexion;
    using PPSale.Models.Globals;
    using PPSale.Models.View;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    public class ContentControlController : Controller
    {
        private ConexionContext db = new ConexionContext();

        // GET: ContentCotrol
        public ActionResult Index()
        {
            var fullControl = new ContentControlViewModel() {
                ivaConditions =db.IvaConditions.ToList(),
                typePayments= db.TypePayments.ToList(),
                typeDocuments= db.TypeDocuments.ToList()
            };

            return View(fullControl);
        }

        //AddIvaCondition
        public ActionResult AddIvaCondition()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddIvaCondition(IvaCondition generic)
        {
            if (ModelState.IsValid)
            {
                db.IvaConditions.Add(generic);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    TempData["Action"] = "Success";
                    TempData["Message"] = "Guardado Exitosamente!!!";

                    return RedirectToAction("index");
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

        public ActionResult EditIvaCondition(int? id)
        {
            if (id == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No se envión ID...";

                return RedirectToAction("Index");
            }
            var n = db.IvaConditions.Find(id);
            if (n == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No existe regitro con este ID...";

                return RedirectToAction("Index");
            }
            return PartialView(n);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditIvaCondition(IvaCondition Generic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Generic).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    TempData["Action"] = "Success";
                    TempData["Message"] = "Actualizado Exitosamente!!!";

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
            return View(Generic);
        }

        //TypePayment
        public ActionResult AddTypePayment()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTypePayment(TypePayment generic)
        {
            if (ModelState.IsValid)
            {
                db.TypePayments.Add(generic);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    TempData["Action"] = "Success";
                    TempData["Message"] = "Guardado Exitosamente!!!";

                    return RedirectToAction("index");
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

        public ActionResult EditTypePayment(int? id)
        {
            if (id == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No se envión ID...";

                return RedirectToAction("Index");
            }
            var n = db.TypePayments.Find(id);
            if (n == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No existe regitro con este ID...";

                return RedirectToAction("Index");
            }
            return PartialView(n);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTypePayment(TypePayment Generic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Generic).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    TempData["Action"] = "Success";
                    TempData["Message"] = "Actualizado Exitosamente!!!";

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
            return View(Generic);
        }
        
        //TypeDocument
        public ActionResult AddTypeDocument()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTypeDocument(TypeDocument generic)
        {
            if (ModelState.IsValid)
            {
                db.TypeDocuments.Add(generic);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    TempData["Action"] = "Success";
                    TempData["Message"] = "Guardado Exitosamente!!!";

                    return RedirectToAction("index");
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

        public ActionResult EditTypeDocument(int? id)
        {
            if (id == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No se envión ID...";

                return RedirectToAction("Index");
            }
            var n = db.TypeDocuments.Find(id);
            if (n == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No existe regitro con este ID...";

                return RedirectToAction("Index");
            }
            return PartialView(n);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTypeDocument(TypeDocument Generic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Generic).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    TempData["Action"] = "Success";
                    TempData["Message"] = "Actualizado Exitosamente!!!";

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
            return View(Generic);
        }

        //TypeDocumentWithCategoryDocument
        public ActionResult TDWithCD(int? id)
        {
            if (id == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No se envión ID...";

                return RedirectToAction("Index");
            }
            var generic = db.TypeDocuments.Include(i=>i.CategoryDocuments).Where(i=>i.TypeDocumentId==id).FirstOrDefault();
            if (generic == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No existe regitro con este ID...";

                return RedirectToAction("Index");
            }

            var gen = new TDWithCDViewModel()
            {
                TypeDocumentId = generic.TypeDocumentId,
                Name = generic.Name,
                categoryDocuments = generic.CategoryDocuments
            };

            return View(gen);
        }

        public ActionResult AddCatDoc(int? Id)
        {
            if (Id == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No se envión ID...";

                return RedirectToAction("TDWithCD", "ContentControl", new { id = Id });
            }
            var n = db.TypeDocuments.Find(Id);
            if (n == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No existe regitro con este ID...";

                return RedirectToAction("TDWithCD", "ContentControl", new { id = Id });
            }

            var x = new CategoryDocument() {
                TypeDocumentId = Id.Value
            };
            return PartialView(x);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCatDoc(CategoryDocument generic)
        {
            if (ModelState.IsValid)
            {
                db.CategoryDocuments.Add(generic);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    TempData["Action"] = "Success";
                    TempData["Message"] = "Guardado Exitosamente!!!";

                    return RedirectToAction("TDWithCD", "ContentControl", new { id = generic.TypeDocumentId });
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

            return RedirectToAction("TDWithCD", "ContentControl", new { id = generic.TypeDocumentId });
        }

        public ActionResult EditCatDoc(int? Idt, int? Idc)
        {
            if (Idt == null || Idc==null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No se envión ID...";

                return RedirectToAction("TDWithCD", "ContentControl", new { id = Idt });
            }
            var n = db.CategoryDocuments.Find(Idc);
            if (n == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No existe regitro con este ID...";

                return RedirectToAction("TDWithCD", "ContentControl", new { id = Idt });
            }
            return PartialView(n);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCatDoc(CategoryDocument Generic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Generic).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    TempData["Action"] = "Success";
                    TempData["Message"] = "Actualizado Exitosamente!!!";

                    return RedirectToAction("TDWithCD", "ContentControl", new { id = Generic.TypeDocumentId });
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
            return View(Generic);
        }
    }
}