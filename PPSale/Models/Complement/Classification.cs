using PPSale.Models.Globals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPSale.Models.Complement
{
    public class Classification
    {
        [Key]
        public int ClassificationId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(50, ErrorMessage = "El Campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Clasificación")]
        [Index("Classification_NameCompanyId_Index", 2, IsUnique = true)]
        public string Name { get; set; }

        //Relacion
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Compañia")]
        [Index("Classification_NameCompanyId_Index", 1, IsUnique = true)]
        public int CompanyId { get; set; }

        //
        public virtual Company Company { get; set; }

        //
        public virtual ICollection<ProductClassification> ProductClassifications { get; set; }
    }
}