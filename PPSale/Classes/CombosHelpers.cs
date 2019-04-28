using PPSale.Models.Complement;
using PPSale.Models.Conexion;
using PPSale.Models.Departure;
using PPSale.Models.Entry;
using PPSale.Models.Globals;
using PPSale.Models.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PPSale.Classes
{
    public class CombosHelpers
    {
        private static ConexionContext db = new ConexionContext();

        //Pais
        public static List<Country> GetCountries()
        {
            var countries = db.Countries.ToList();
            countries.Add(new Country { CountryId = 0, Name = "[Seleccionar...]" });
            return countries.OrderBy(p => p.Name).ToList();
        }
        
        //Provincia
        
        public static List<Province> GetProvinces(int countryId)
        {
            var province = db.Provinces.Where(p=>p.CountryId== countryId).ToList();
            province.Add(new Province { ProvinceId = 0, Name = "[Seleccionar...]" });
            return province.OrderBy(p => p.Name).ToList();
        }

        //Ciudad
        public static List<City> GetCities(int countryId, int provinceId)
        {
            var cities = db.Cities.Where(p => p.CountryId == countryId && p.ProvinceId== provinceId).ToList();
            cities.Add(new City { CityId = 0, Name = "[Seleccionar...]" });
            return cities.OrderBy(p => p.Name).ToList();
        }

        //Condicion de iva
        public static List<IvaCondition> GetIvaConditions()
        {
            var iva = db.IvaConditions.ToList();
            iva.Add(new IvaCondition { IvaConditionId = 0, Name = "[Seleccionar...]" });
            return iva.OrderBy(p => p.Name).ToList();
        }

        //Rol
        public static List<Rol> GetRols()
        {
            var rol = db.Rols.ToList();
            rol.Add(new Rol { RolId = 0, Name = "[Seleccionar...]" });
            return rol.OrderBy(p => p.Name).ToList();
        }
        
        //Usuarios
        public static List<ViewUser> GetUsers()
        {
            var user = db.Users.ToList();

            var viewUser = new List<ViewUser>();
            viewUser.Add(new ViewUser { UserId = 0, UserName = "[Seleccionar...]" });

            foreach (var item in user)
            {
                viewUser.Add(new ViewUser() { UserId=item.UserId, UserName = item.FirstName +" "+ item.LastName + " - " + item.UserName});
            }

            return viewUser.OrderBy(v=>v.UserName).ToList();
        }

        //Compañia
        public static List<Company> Getcompanies()
        {
            var company = db.Companies.ToList();
            company.Add(new Company { CompanyId = 0, Name = "[Seleccionar...]" });
            return company.OrderBy(p => p.Name).ToList();
        }

        //tipo de documento
        public static List<TypeDocument> GetTypeDocuments()
        {
            var c = db.TypeDocuments.ToList();
            c.Add(new TypeDocument { TypeDocumentId = 0, Name = "[Seleccionar...]" });
            return c.OrderBy(p => p.Name).ToList();
        }

        //Categoria de documento
        public static List<CategoryDocument> GetCategoryDocuments(int id)
        {
            var c = db.CategoryDocuments.Where(o=>o.TypeDocumentId==id).ToList();
            c.Add(new CategoryDocument { CategoryDocumentId = 0, Name = "[Seleccionar...]" });
            return c.OrderBy(p => p.Name).ToList();
        }


        //Cliente
        public static List<Client> GetClients(int id)
        {
            var c = db.Clients.Where(d=>d.CompanyId==id).ToList();
            c.Add(new Client { ClientId = 0, Name = "[Seleccionar...]" });
            return c.OrderBy(p => p.Name).ToList();
        }

        //Provider
        public static List<Provider> GetProviders(int id)
        {
            var c = db.Providers.Where(d => d.CompanyId == id).ToList();
            c.Add(new Provider { ProviderId = 0, Name = "[Seleccionar...]" });
            return c.OrderBy(p => p.Name).ToList();
        }

        //tipo de pago
        public static List<TypePayment> GetTypePayments()
        {
            var c = db.TypePayments.ToList();
            c.Add(new TypePayment { TypePaymentId = 0, Name = "[Seleccionar...]" });
            return c.OrderBy(p => p.Name).ToList();
        }

        //Clasificacion
        public static List<Classification> GetClassifications(int Idp, int Idc)
        {
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

            classification.Add(new Classification { ClassificationId = 0, Name = "[Seleccionar...]" });
            return classification.OrderBy(p => p.Name).ToList();
        }

        //Clasificacion
        public static List<Classification> GetClassificationsAll(int Idc)
        {
            var c = db.Classifications.Where(q => q.CompanyId == Idc).ToList();
            c.Add(new Classification { ClassificationId = 0, Name = "[Seleccionar...]" });
            return c.OrderBy(p => p.Name).ToList();
        }

        //Productos
        public static List<Product> GetProducts(int Id)
        {
            var c = db.Products.Where(q => q.CompanyId == Id).ToList();
            c.Add(new Product { ProductId = 0, Name = "[Seleccionar...]" });
            return c.OrderBy(p => p.Name).ToList();
        }


        //Producto Clasificacion
        internal static List<ProductClassification> GetProductClassification(int IdC)
        {
            var c = db.ProductClassifications.Where(q => q.CompanyId == IdC).ToList();
            
            var ClassP = new List<ProductClassification>();
            foreach (var item in c)
            {
                ClassP.Add(new ProductClassification() {
                    ProductClassificationId=item.ProductClassificationId,
                    CodeBar = item.CodeBar,
                    ProductId = item.ProductId,
                    ClassificationId = item.ClassificationId,
                    CompanyId = item.CompanyId,
                  // Product = $"{item.Product.Name}{' '}{item.Classification.Name}",
                });
            }

            //ClassP.Add(new ProductClassification { ProductClassificationId = 0, ProductFull = "[Seleccionar...]" });

            return ClassP.OrderBy(p => p.ProductFull).ToList();
        }

        //Unidad de medida
        public static List<MeasuredUnit> GetMeasuredUnit(int Id)
        {
            var c = db.MeasuredUnits.Where(q => q.CompanyId == Id).ToList();
            c.Add(new MeasuredUnit { MeasuredUnitId = 0, Name = "[Seleccionar...]" });
            return c.OrderBy(p => p.Name).ToList();
        }

        public static List<UnitBase> GetUnitBase()
        {
            var c = db.UnitBases.ToList();
            c.Add(new UnitBase { UnitBaseId = 0, Name = "[Seleccionar...]" });
            return c.OrderBy(p => p.Name).ToList();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}