using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PPSale.Models.Globals
{
    public class IvaCondition
    {
        [Key]
        public int IvaConditionId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(50, ErrorMessage = "El Campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Condición Iva")]
        [Index("IvaCondition_Name_Index", IsUnique = true)]
        public string Name { get; set; }

        //VIRTUALIZAR
        public virtual ICollection<Company> Companies { get; set; }
        //public virtual ICollection<Province> Providers { get; set; }
    }
}