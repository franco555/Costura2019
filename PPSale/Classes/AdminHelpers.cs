using PPSale.Models.Conexion;
using PPSale.Models.Globals;
using System;
using System.Linq;
using System.Web.WebPages;

namespace PPSale.Classes
{

    public class AdminHelpers:IDisposable
    {
        private static ConexionContext db = new ConexionContext();

        public static bool IngresarDatosAdmin()
        {
            var ok=Compania();
            if (ok!=0)
            {
                return true;
            }
            return false;
        }

        public static int Pais()
        {
            var name = "Perú";
            var pais = new Country { CountryId = 0, Name = name, };

            var seach = db.Countries.Where(s => s.Name == name).ToList();

            if (seach == null || !seach.Any())
            {
                db.Countries.Add(pais);
                db.SaveChanges();
                return pais.CountryId;
            }
            else
            {
                var res = seach.Find(d => d.Name.Contains(name));
                return res.CountryId;
            }


        }

        public static int Provincia(int Id)
        {
            var name = "Junin";
            var province = new Province { ProvinceId = 0, Name = name, CountryId = Id, };

            var seach = db.Provinces.Where(s => s.Name == name && s.CountryId==Id).ToList();
            
            if (seach == null || !seach.Any())
            {
                db.Provinces.Add(province);
                db.SaveChanges();
                return province.ProvinceId;
            }
            else
            {
                var res = seach.Find(d => d.Name.Contains(name) && d.CountryId == Id);
                return res.ProvinceId;
            }

            
        }

        public static int Ciudad()
        {
            var Coun = Pais();
            var Prov = Provincia(Coun);
            var name = "San Matín de Pangoa";
            var cities = new City { CityId = 0, Name = name, CountryId = Coun, ProvinceId = Prov, };

            var seachd = db.Cities.Where(s => s.Name ==name && s.CountryId == Coun && s.ProvinceId == Prov).ToList();
            if (seachd == null || !seachd.Any())
            {
                db.Cities.Add(cities);
                db.SaveChanges();
                return cities.CityId;
            }
            else
            {
                var res = seachd.Find(d => d.Name.Contains(name) && d.CountryId==Coun && d.ProvinceId==Prov);
                return res.CityId;
            }
        }

        public static int Ivas()
        {
            var name = "No Inscripto";
            var ivas = new IvaCondition { IvaConditionId = 0, Name = name, };
            var seachi = db.IvaConditions.Where(s => s.Name == name).ToList();
            if (seachi == null || !seachi.Any())
            {
                db.IvaConditions.Add(ivas);
                db.SaveChanges();
                return ivas.IvaConditionId;
            }
            else
            {
                var res = seachi.Find(d => d.Name.Contains(name));
                return res.IvaConditionId;
            }
        }

        public  static int Compania()
        {
            var name = "Anónimo";
            DateTime dt5 = "1989-10-16".AsDateTime();
            var company = new Company
            {
                CompanyId = 0,
                Name = name,
                Cuit = "20-95488417",
                Address = "La villa 1-11-14 Mz6 - Casa 34",
                Email = "razon20m@gmail.com",
                Web = "www.?.com",
                FirstDate = dt5,
                Logo = null,
                IvaConditionId = Ivas(),
                CountryId = Pais(),
                ProvinceId = Provincia(Pais()),
                CityId = Ciudad(),

            };
            var seachp = db.Companies.Where(s => s.Name == name).ToList();
            if (seachp == null || !seachp.Any())
            {
                db.Companies.Add(company);
                db.SaveChanges();
                return company.CompanyId;
            }
            else
            {
                var res = seachp.Find(d => d.Name.Contains(name));
                return res.CompanyId;
            }
            
        }

        public static int Usuario()
        {
            var username = "razon20m@gmail.com";
            var user = new User
            {
                UserId = 0,
                UserName = username,
                FirstName = "Franco",
                LastName = "Caysahuana Ch.",
                Cuit = "20-95488417-2",
                Phone = "1122344991",
                Address = "La villa 1-11-14 Mz6 - Casa 34",
                Logo = "~/Content/Logos/Empresa/otro.png",
                CountryId= Pais(),
                ProvinceId = Provincia(Pais()),
                CityId = Ciudad(),

            };

            var seachu = db.Users.Where(s => s.UserName ==  username).ToList();
            if (seachu == null || !seachu.Any())
            {
                db.Users.Add(user);
                db.SaveChanges();
                return user.UserId;
            }
            else
            {
                var res = seachu.Find(d => d.UserName.Contains(username));
                return res.UserId;
            }
        }

      
    
        public void Dispose()
        {
            db.Dispose();
        }
    }
}