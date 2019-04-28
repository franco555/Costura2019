﻿using PPSale.Models.Conexion;
using PPSale.Models.Globals;
using PPSale.Models.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.WebPages;

namespace PPSale.Classes
{
    public class FunctionHelpers
    {
        private static ConexionContext db = new ConexionContext();

        public string notUser { get; set; }
        public string notCompany { get; set; }
        public string notImage { get; set; }
        private string urlBase { get; set; }

        public User user { get; set; }
        public Company company { get; set; }
        public int CityId { get; set; }
        public int CompanyId { get; set; }

        public string SuccessCreate { get; set; }
        public string SuccessUpdate { get; set; }

        public string notRegistre{get; set;}
        public string notId { get; set; }
        public string notExist { get; set; }

        public FunctionHelpers()
        {
            urlBase = "~/Content/Logos/";
            notUser = $"{urlBase}NotUser.jpg";
            notCompany = $"{urlBase}NotCompany.jpg";
            notImage = $"{urlBase}NotImage.jpg";
            CityId = Ciudad();

            user = new User
            {
                UserId = 0,
                UserName = "razon20m@gmail.com",
                FirstName = "Franco",
                LastName = "Caysahuana Ch.",
                Cuit = "20-95488417-2",
                Phone = "1122344991",
                Address = "La villa 1-11-14 Mz6 - Casa 34",
                Logo = notUser,
                CountryId = 1, //NotMatped
                ProvinceId = 1, //NotMatped
                CityId = CityId,

            };

            company = new Company
            {
                CompanyId = 0,
                Name = "PrograMagic",
                Cuit = "20-95488417",
                Address = "La villa 1-11-14 Mz6 - Casa 34",
                Email = "razon20m@gmail.com",
                Web = "www.ProgaMagic.com",
                FirstDate = Convert.ToDateTime("1989-10-16"),
                Logo = notCompany,
                IvaConditionId = Ivas(),
                CountryId = 1, //NotMatped
                ProvinceId = 1, //NotMatped
                CityId = CityId,

            };

            CompanyId = Compania(company);

            //Mensajes
            SuccessCreate = "Felicitaciones! Registro Guardado Correctamente.";
            SuccessUpdate = "Felicitaciones! Registro Actualizado Correctamente.";

            notRegistre = "Necesita que el usuario este registado en alguna empresa, Éste usuario no tiene permiso de Modificar esta opción. Por favor comuniquese con el administrador";
            notId = "No se recibio ningun ID.";
            notExist = "No existe regsitro!";
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

            var seach = db.Provinces.Where(s => s.Name == name && s.CountryId == Id).ToList();

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

            var seachd = db.Cities.Where(s => s.Name == name && s.CountryId == Coun && s.ProvinceId == Prov).ToList();
            if (seachd == null || !seachd.Any())
            {
                db.Cities.Add(cities);
                db.SaveChanges();
                return cities.CityId;
            }
            else
            {
                var res = seachd.Find(d => d.Name.Contains(name) && d.CountryId == Coun && d.ProvinceId == Prov);
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

        public int Compania(Company companies)
        {
            try
            {
                var comp = db.Companies.Where(c => c.Name == companies.Name).FirstOrDefault();
                if (comp == null)
                {
                    db.Companies.Add(companies);
                    var response = DBHelpers.SaveChage(db);

                    return companies.CompanyId;
                }

                return comp.CompanyId;
            }
            catch (Exception ex)
            {

                var e = new Response() { Message = ex.Message };
                return 0;
            }

        }
    }
}