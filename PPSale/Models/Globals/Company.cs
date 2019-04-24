using PPSale.Models.Complement;
using PPSale.Models.Departure;
using PPSale.Models.Entry;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PPSale.Models.Globals
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(50, ErrorMessage = "El Campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Compañia")]
        [Index("Company_Name_CUIT_Index", 2, IsUnique = true)]
        public string Name { get; set; }

        [MaxLength(50, ErrorMessage = "El Campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "CUIT/CUIL")]
        [Index("Company_Name_CUIT_Index", 1, IsUnique = true)]
        public string Cuit { get; set; }

        [MaxLength(100, ErrorMessage = "El Campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Direccion")]
        public string Address { get; set; }

        [MaxLength(100, ErrorMessage = "El Campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MaxLength(100, ErrorMessage = "El Campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Sitio Web")]
        public string Web { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Inicio")]
        public DateTime FirstDate { get; set; }

        [Display(Name = "Logo")]
        public string Logo { get; set; }

        [NotMapped]
        [Display(Name = "Logo")]
        public HttpPostedFileBase LogoFile { get; set; }

        [NotMapped]
        public string URL { get; set; }

        //RELACION
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Condición Iva")]
        public int IvaConditionId { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "País")]
        public int CountryId { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Provincia")]
        public int ProvinceId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Ciudad")]
        public int CityId { get; set; }

        public virtual IvaCondition IvaCondition { get; set; }
        //public virtual Country Country { get; set; }
        //public virtual Province Province { get; set; }
        public virtual City City { get; set; }

        //Relaciones
        public virtual ICollection<AsingRolAndUser> AsingRolAndUsers { get; set; }
        public virtual ICollection<Provider> Providers { get; set; }
        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<MeasuredUnit> MeasuredUnits { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Classification> Classifications { get; set; }
        public virtual ICollection<DocumentEntry> DocumentEntries { get; set; }
        public virtual ICollection<DocumentEntryDetail> DocumentEntryDetails { get; set; }
        public virtual ICollection<ProductClassification> ProductClassifications { get; set; }
        public virtual ICollection<UnitBase> UnitBases { get; set; }
    }
}