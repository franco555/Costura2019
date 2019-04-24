namespace PPSale.Models.Globals
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Country
    {
        [Key]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(50, ErrorMessage = "El Campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Pais")]
        [Index("Country_Name_Index", IsUnique = true)]
        public string Name { get; set; }

        [NotMapped]
        [Display(Name = "# Provincias")]
        public int NumberProvinces { get { return Provinces == null ? 0 : Provinces.Count; } }

        //Relaciones
        public virtual ICollection<Province> Provinces { get; set; }
        //public virtual ICollection<City> Cities { get; set; }
        //public virtual ICollection<User> Users { get; set; }
        //public virtual ICollection<Company> Companies { get; set; }
    }
}