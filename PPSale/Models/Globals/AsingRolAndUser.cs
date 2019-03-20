using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PPSale.Models.Globals
{
    public class AsingRolAndUser
    {
        [Key]
        public int AsingRolAndUserId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Compañia")]
        [Index("AsingRolAndUser_CID_UID_RID_Index", 1, IsUnique = true)]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Usuario")]
        [Index("AsingRolAndUser_CID_UID_RID_Index", 2, IsUnique = true)]
        public int UserId { get; set; }

        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Rol")]
        public int RolId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(256, ErrorMessage = "El Campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Activo")]
        public bool Lock { get; set; }

        public virtual Company Company { get; set; }
        public virtual User User { get; set; }
        public virtual Rol Rol { get; set; }
    }
}