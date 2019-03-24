namespace PPSale.Models.Globals
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Province
    {
        [Key]
        public int ProvinceId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(50, ErrorMessage = "El Campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Provincia")]
        [Index("Province_Name_CountryId_Index", 2, IsUnique = true)]
        public string Name { get; set; }

        [NotMapped]
        [Display(Name = "# Ciudades")]
        public int NumberCities { get { return Cities == null ? 0 : Cities.Count; } }

        //Ids TABLAS
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Pais")]
        [Index("Province_Name_CountryId_Index", 1, IsUnique = true)]
        public int CountryId { get; set; }
        
        //Relacion
        public virtual Country Country { get; set; }

        //Relaciones
        public virtual ICollection<City> Cities { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
    }
}