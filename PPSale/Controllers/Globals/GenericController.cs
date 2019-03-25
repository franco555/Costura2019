using PPSale.Classes;
using PPSale.Models.Complement;
using PPSale.Models.Conexion;
using PPSale.Models.View;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace PPSale.Controllers.Global
{
    public class GenericController : Controller
    {
        private ConexionContext db = new ConexionContext();

        //DELETE----------------------------------------------------------------------------

        //DeleteCountry
        public JsonResult DeleteCountry(int? Id)
        {
            db.Configuration.ProxyCreationEnabled = false;

            if (Id==null){return Json("Id Incorrecto!!!");}

            var country = db.Countries.Find(Id);

            if (country == null) { return Json("El Registro no existe!!!"); }

            db.Countries.Remove(country);

            var response = DBHelpers.SaveChage(db);

            if (response.Succeded)
            {
                return Json("OK");
            }

            return Json(response.Message);
        }

        //DeleteProvince
        public JsonResult DeleteProvince(int? Id)
        {
            db.Configuration.ProxyCreationEnabled = false;

            if (Id == null) { return Json("Id Incorrecto!!!"); }

            var province = db.Provinces.Find(Id);

            if (province == null) { return Json("El Registro no existe!!!"); }

            db.Provinces.Remove(province);

            var response = DBHelpers.SaveChage(db);

            if (response.Succeded)
            {
                return Json("OK");
            }

            return Json(response.Message);
        }

        //DeleteCity
        public JsonResult DeleteCity(int? Id)
        {
            db.Configuration.ProxyCreationEnabled = false;

            if (Id == null) { return Json("Id Incorrecto!!!"); }

            var city = db.Cities.Find(Id);

            if (city == null) { return Json("El Registro no existe!!!"); }

            db.Cities.Remove(city);

            var response = DBHelpers.SaveChage(db);

            if (response.Succeded)
            {
                return Json("OK");
            }

            return Json(response.Message);
        }

        //DeleteIva
        public JsonResult DeleteIvaCondition(int? Id)
        {
            db.Configuration.ProxyCreationEnabled = false;

            if (Id == null) { return Json("Id Incorrecto!!!"); }

            var iva = db.IvaConditions.Find(Id);

            if (iva == null) { return Json("El Registro no existe!!!"); }

            db.IvaConditions.Remove(iva);

            var response = DBHelpers.SaveChage(db);

            if (response.Succeded)
            {
                return Json("OK");
            }

            return Json(response.Message);
        }

        //DeleteTypePayment
        public JsonResult DeleteTypePayment(int? Id)
        {
            db.Configuration.ProxyCreationEnabled = false;

            if (Id == null) { return Json("Id Incorrecto!!!"); }

            var payment = db.TypePayments.Find(Id);

            if (payment == null) { return Json("El Registro no existe!!!"); }

            db.TypePayments.Remove(payment);

            var response = DBHelpers.SaveChage(db);

            if (response.Succeded)
            {
                return Json("OK");
            }

            return Json(response.Message);
        }

        //Delete Unidad de medida
        public JsonResult DeleteTypeDocument(int? Id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if (Id == null) { return Json("Id Incorrecto!!!"); }

            var n = db.TypeDocuments.Find(Id);
            if (n == null) { return Json("El Registro no existe!!!"); }
            db.TypeDocuments.Remove(n);

            var response = DBHelpers.SaveChage(db);

            if (response.Succeded)
            {
                return Json("OK");
            }

            return Json(response.Message);
        }

        //Delete Unidad de medida
        public JsonResult DeleteCategoryDocument(int? Id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if (Id == null) { return Json("Id Incorrecto!!!"); }

            var n = db.CategoryDocuments.Find(Id);
            if (n == null) { return Json("El Registro no existe!!!"); }

            db.CategoryDocuments.Remove(n);

            var response = DBHelpers.SaveChage(db);

            if (response.Succeded)
            {
                return Json("OK");
            }

            return Json(response.Message);
        }

        //Delete Company
        public JsonResult DeleteCompany(int Id)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var company = db.Companies.Find(Id);
            db.Companies.Remove(company);

            var response = DBHelpers.SaveChage(db);

            if (response.Succeded)
            {
                /* var FileToDelete= "~/Content/Logos";
                 var FileDelete = Server.MapPath(FileToDelete + Id+".jpg");

                 System.IO.File.Delete(FileToDelete);*/

                return Json("OK");
            }

            return Json(response.Message);


        }
        //Delete Rol
        public JsonResult DeleteRol(int Id)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var rol = db.Rols.Find(Id);
            db.Rols.Remove(rol);

            var response = DBHelpers.SaveChage(db);

            if (response.Succeded)
            {
                return Json("OK");
            }

            return Json(response.Message);


        }

        //Delete User
        public JsonResult DeleteUser(int Id)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var rol = db.Users.Find(Id);
            db.Users.Remove(rol);

            var response = DBHelpers.SaveChage(db);

            if (response.Succeded)
            {
                return Json("OK");
            }

            return Json(response.Message);


        }

        //Delete AsigRol And User
        public JsonResult DeleteAsingRolAndUser(int Id)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var asig = db.AsingRolAndUsers.Find(Id);
            db.AsingRolAndUsers.Remove(asig);

            var response = DBHelpers.SaveChage(db);

            if (response.Succeded)
            {
                var rol = db.Rols.Find(asig.RolId);
                var userName = db.Users.Find(asig.UserId);
                UsersHelper.RemoveRol(userName.UserName, rol.Name);

                UsersHelper.DeleteUser(asig.Email);

                return Json("OK");
            }

            return Json(response.Message);


        }

        //Delete Cliente
        public JsonResult DeleteClient(int Id)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var n = db.Clients.Find(Id);
            db.Clients.Remove(n);

            var response = DBHelpers.SaveChage(db);

            if (response.Succeded)
            {
                return Json("OK");
            }

            return Json(response.Message);
        }

        //Delete Provider
        public JsonResult DeleteProvider(int Id)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var n = db.Providers.Find(Id);
            db.Providers.Remove(n);

            var response = DBHelpers.SaveChage(db);

            if (response.Succeded)
            {
                return Json("OK");
            }

            return Json(response.Message);
        }

        //Delete Unidad de medida
        public JsonResult DeleteMeasuredUnit(int Id)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var n = db.MeasuredUnits.Find(Id);
            db.MeasuredUnits.Remove(n);

            var response = DBHelpers.SaveChage(db);

            if (response.Succeded)
            {
                return Json("OK");
            }

            return Json(response.Message);
        }

        //Delete documento de compra
        public JsonResult DeleteDocumentEntry(int Id)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var n = db.DocumentEntries.Find(Id);
            db.DocumentEntries.Remove(n);

            var response = DBHelpers.SaveChage(db);

            if (response.Succeded)
            {
                return Json("OK");
            }

            return Json(response.Message);
        }





        //GET----------------------------------------------------------------------------

        //Para Obtener una departamento de acuerdo al paiss
        public JsonResult GetProvinces(int Id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var province = db.Provinces.Where(m => m.CountryId == Id).OrderBy(m => m.Name);
            var json = Json(province);

            return json;

        }

        //Para Obtener Ciudades
        public JsonResult GetCities(int IdC, int IdP)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var cities = db.Cities.Where(m => m.CountryId == IdC && m.ProvinceId == IdP).OrderBy(m => m.Name);
            var json = Json(cities);
            return json;

        }

        //Para Obtener Categoria de Documento
        public JsonResult GetCategoryDocuments(int Id)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var a = db.CategoryDocuments.Where(m => m.TypeDocumentId == Id).OrderBy(m => m.Name);
            var json = Json(a);
            return json;

        }

        //Para Obtener Classificacion de producto
        public JsonResult GetClassifications(int Idp, int Idc)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var query = (from p in db.Products
                         join pc in db.ProductClassifications on p.ProductId equals pc.ProductId
                         join c in db.Classifications on pc.ClassificationId equals c.ClassificationId
                         where p.ProductId == Idp && p.CompanyId == Idc

                         select new { c }).ToList();

            var classification = new List<Classification>();
            foreach (var item in query)
            {
                classification.Add(new Classification() { ClassificationId = item.c.ClassificationId, Name = item.c.Name, CompanyId = item.c.CompanyId });
            }

            var json = Json(classification.OrderBy(o => o.Name).ToList());
            return json;

        }

        //Para Obtener Unidad de medida 
        public JsonResult GetMeasuredUnits(int IdPr, int IdCl, int IdCo)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var query = (from pc in db.ProductClassifications
                         join k in db.Kardexes on pc.ProductClassificationId equals k.ProductClassificationId
                         join um in db.MeasuredUnits on k.MeasuredUnitId equals um.MeasuredUnitId
                         where pc.ProductId == IdPr && pc.ClassificationId==IdCl && pc.CompanyId == IdCo 

                         select new { um}).ToList();

            var measuredUnit = new List<MeasuredUnit>();
            foreach (var item in query)
            {
                measuredUnit.Add(new MeasuredUnit() {
                    MeasuredUnitId = item.um.MeasuredUnitId,
                    Name = $"{item.um.Name}{' '}{'['}{item.um.Abrev}{']'}",
                    CompanyId = item.um.CompanyId,
                    
                });
            }

            var json = Json(measuredUnit.OrderBy(o => o.Name).ToList());
            return json;

        }

        //Para Precio
        public JsonResult GetPrice(int IdPr, int IdMe,int IdCl, int IdCo)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var query = (from pc in db.ProductClassifications
                         join k in db.Kardexes on pc.ProductClassificationId equals k.ProductClassificationId
                         where 
                         pc.ProductId == IdPr && 
                         pc.ClassificationId == IdCl && 
                         pc.CompanyId == IdCo && 
                         k.MeasuredUnitId==IdMe

                         select new { k }).ToList();

            var price = new List<ViewPrice>();
            foreach (var item in query)
            {
                price.Add(new ViewPrice() { ProductClassificationId = item.k.ProductClassificationId, Price = item.k.Price, });
            }

            double Precio = 0;
            if (price.Count > 0)
            {
                Precio = price[0].Price;
            }

            var s = Precio.ToString().Replace(".", ",");
            var json = Json(s);
            return json;

        }



        // Guardar
        public JsonResult AddKardex(int Id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            /*
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var de = db.DocumentEntries.Find(Id);

                    var DEDetail = db.DocumentEntryDetails.Where(d => d.DocumentEntryId == de.DocumentEntryId).ToList();
                    double total = 0;

                    foreach (var item in DEDetail)
                    {
                        total += item.Quantity * item.Price;

                        if (item.save == false)
                        {
                            var kardex = db.ProductClassifications.Where(k => k.ProductId == item.ProductId && k.ClassificationId == item.ClassificationId).FirstOrDefault();
                            // save=false   edit=true   otherTbla=true  saveTable=true

                            if (item.SaveTable == true)
                            {
                                var kardexOld = db.ProductClassifications.Where(k => k.ProductId == item.alt_ProductId && k.ClassificationId == item.alt_ClassificationId).FirstOrDefault();
                                kardexOld.Stock -= item.alt_Quantity;
                                db.Entry(kardexOld).State = EntityState.Modified;
                            }

                            kardex.Stock += item.Quantity;
                            db.Entry(kardex).State = EntityState.Modified;


                            item.save = true;
                            item.Edit = false;
                            item.OtherTable = false;
                            item.SaveTable = false;
                            db.Entry(item).State = EntityState.Modified;

                        }
                        else
                        {

                            double mont = 0;
                            if (item.Edit == true)
                            {
                                //otherTbla=true  saveTable=false
                                //p=total   p2=otrher table
                                if (item.SaveTable == false)
                                {
                                    var kardex = db.ProductClassifications.Where(k =>
                                        k.ProductId == item.ProductId &&
                                        k.ClassificationId == item.ClassificationId)
                                        .FirstOrDefault();

                                    mont = item.Quantity - item.alt_Quantity;

                                    kardex.Stock += mont;
                                    db.Entry(kardex).State = EntityState.Modified;

                                }
                                else
                                {
                                    var kardexOld = db.ProductClassifications.Where(k =>
                                    k.ProductId == item.alt_ProductId &&
                                    k.ClassificationId == item.alt_ClassificationId)
                                    .FirstOrDefault();

                                    kardexOld.Stock -= item.alt_Quantity;
                                    db.Entry(kardexOld).State = EntityState.Modified;

                                    var kardex = db.ProductClassifications.Where(k =>
                                    k.ProductId == item.ProductId &&
                                    k.ClassificationId == item.ClassificationId)
                                    .FirstOrDefault();

                                    var newMont = item.Quantity - item.alt_Quantity;
                                    kardex.Stock += item.Quantity - newMont;
                                    db.Entry(kardex).State = EntityState.Modified;

                                }

                                item.save = true;
                                item.Edit = false;
                                item.OtherTable = false;
                                item.SaveTable = false;
                                db.Entry(item).State = EntityState.Modified;
                            }
                        }

                    }


                    de.save = true;
                    de.PriceTotal = total;
                    db.Entry(de).State = EntityState.Modified;


                    db.SaveChanges();
                    transaction.Commit();

                    return Json("OK");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return Json(ex.Message);
                }
            }*/
            return Json("0");

        }

        /*
        public JsonResult DocumetEntryDetailCancel(int Id)
        {
            db.Configuration.ProxyCreationEnabled = false;

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var de = db.DocumentEntries.Find(Id);
                    var DEDetail = db.DocumentEntryDetails.Where(d => d.DocumentEntryId == de.DocumentEntryId).ToList();

                    foreach (var item in DEDetail)
                    {

                        if (item.save == false)
                        {
                            item.ProductId = item.alt_ProductId;
                            item.ClassificationId = item.alt_ClassificationId;
                            item.Quantity = item.alt_Quantity;
                            item.save = true;
                            item.Edit = false;

                            db.Entry(item).State = EntityState.Modified;
                        }
                        else
                        {
                            if (item.Edit == true)
                            {
                                
                                item.ProductId = item.alt_ProductId;
                                item.ClassificationId = item.alt_ClassificationId;
                                item.Quantity = item.alt_Quantity;
                                item.save = true;
                                item.Edit = false;

                                db.Entry(item).State = EntityState.Modified;
                            }
                        }

                    }

                    de.save = true;
                    db.Entry(de).State = EntityState.Modified;


                    db.SaveChanges();
                    transaction.Commit();

                    return Json("OK");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return Json(ex.Message);
                }
            }

        }
        */
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