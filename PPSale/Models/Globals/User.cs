using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace PPSale.Models.Globals
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(256, ErrorMessage = "El Campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Email")]
        [Index("User_UserName_Index", IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(50, ErrorMessage = "El Campo {0} no puede tener más de {1} caracteres.")]
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
        [Display(Name = "Foto")]
        public HttpPostedFileBase LogoFile { get; set; }

        [NotMapped]
        public string URL { get; set; }

        [NotMapped]
        public int companyId { get; set; }

        //Propiedad lectura
        [Display(Name = "Usuario")]
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }



        //RELACION

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
        [Display(Name = "Pais")]
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }
        public virtual Province Province { get; set; }
        public virtual City City { get; set; }

        //Relaciones
        public virtual ICollection<AsingRolAndUser> AsingRolAndUsers { get; set; }
    }
}