using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPSale.Models.Globals
{
    public class Province
    {
        [Key]
        public int ProvinceId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(50, ErrorMessage = "El Campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Provincia")]
        [Index("Province_Name_CountryId_Index", 2, IsUnique = true)]
        public string Name { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Pais")]
        [Index("Province_Name_CountryId_Index", 1, IsUnique = true)]
        public int CountryId { get; set; }

        //public string FullName { get { return string.Concat(Country.Name," - " ,Name); } }

        //RELACIONES ENTRE TABLAS
        public virtual Country Country { get; set; }

        //Relaciones
        public virtual ICollection<City> Cities { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
    }
}