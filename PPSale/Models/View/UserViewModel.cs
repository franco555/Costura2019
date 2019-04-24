using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PPSale.Models.View
{
    public class UserViewModel
    {
        public int userId { get; set; }
        
        [Display(Name = "Email")]
        public string UserName { get; set; }
        
        [Display(Name = "Nombres")]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Display(Name = "CUIT/CUIL")]
        public string Cuit { get; set; }

        [Display(Name = "Teléfono")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Direccion")]
        public string Address { get; set; }

        [Display(Name = "Foto")]
        public string Logo { get; set; }
        
        [NotMapped]
        public int companyId { get; set; }

        [Required]
        [Display(Name = "Rol")]
        [NotMapped]
        public int rolId { get; set; }

        [Display(Name = "País")]
        public string CountryName { get; set; }

        [Display(Name = "Provincia")]
        public string ProvinceName { get; set; }

        [Display(Name = "Ciudad")]
        public string CityName { get; set; }

        //Propiedad lectura
        [Display(Name = "Usuario")]
        public string FullName { get { return $"{FirstName} {LastName}"; } }
    }
}