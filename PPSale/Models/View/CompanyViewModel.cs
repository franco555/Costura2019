using System;
using System.ComponentModel.DataAnnotations;

namespace PPSale.Models.View
{
    public class CompanyViewModel
    {
        public int CompanyId { get; set; }
        
        [Display(Name = "Compañia")]
        public string Name { get; set; }
        
        [Display(Name = "CUIT/CUIL")]
        public string Cuit { get; set; }
        
        [Display(Name = "Direccion")]
        public string Address { get; set; }
        
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Display(Name = "Sitio Web")]
        public string Web { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Inicio")]
        public DateTime FirstDate { get; set; }

        [Display(Name = "Logo")]
        public string Logo { get; set; }
        
        [Display(Name = "Condición Iva")]
        public string IvaConditionName { get; set; }
        
        [Display(Name = "País")]
        public string CountryName { get; set; }
        
        [Display(Name = "Provincia")]
        public string ProvinceName { get; set; }
        
        [Display(Name = "Ciudad")]
        public string CityName { get; set; }

    }
}