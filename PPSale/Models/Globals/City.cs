using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPSale.Models.Globals
{
    public class City
    {
        [Key]
        public int CityId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(50, ErrorMessage = "El Campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Ciudad")]
        [Index("City_Name_CountryId_ProvinceId_Index", 3, IsUnique = true)]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Pais")]
        [Index("City_Name_CountryId_ProvinceId_Index", 1, IsUnique = true)]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Provincia")]
        [Index("City_Name_CountryId_ProvinceId_Index", 2, IsUnique = true)]
        public int ProvinceId { get; set; }

        //public string FullName { get { return string.Format("{0} {1} {2} {3} {4}", Country.Name, " - ", Province.Name, " - ", Name); } }


        //RELACIONES ENTRE TABLAS

        public virtual Country Country { get; set; }
        public virtual Province Province { get; set; }

        //Relaciones
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
    }
}