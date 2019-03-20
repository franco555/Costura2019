using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPSale.Models.Globals
{
    public class Rol
    {
        [Key]
        public int RolId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(50, ErrorMessage = "El Campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Rol")]
        [Index("Rol_Name_Index", IsUnique = true)]
        public string Name { get; set; }

        [Display(Name = "Bloqueado")]
        public bool Lock { get; set; }

        //VIRTUALIZAR
        public virtual ICollection<AsingRolAndUser> AsingRolAndUsers { get; set; }
    }
}