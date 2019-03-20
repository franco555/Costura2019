using PPSale.Models.Globals;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace PPSale.Models.Entry
{
    public class Provider
    {
        [Key]
        public int ProviderId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(50, ErrorMessage = "El Campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Proveedor")]
        [Index("Provider_Name_CUIT_Index", 2, IsUnique = true)]
        public string Name { get; set; }

        [MaxLength(50, ErrorMessage = "El Campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "CUIT/CUIL")]
        [Index("Provider_Name_CUIT_Index", 1, IsUnique = true)]
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

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "País")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Provincia")]
        public int ProvinceId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Ciudad")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Compañia")]
        public int CompanyId { get; set; }

        public virtual IvaCondition IvaCondition { get; set; }
        public virtual Country Country { get; set; }
        public virtual Province Province { get; set; }
        public virtual City City { get; set; }
        public virtual Company Company { get; set; }

        //Relaciones
    }
}