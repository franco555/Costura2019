using PPSale.Models.Entry;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPSale.Models.Globals
{
    public class CategoryDocument
    {
        [Key]
        public int CategoryDocumentId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(50, ErrorMessage = "El Campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Categoría de Doc.")]
        [Index("CategoryDocument_Id_Name_Index", 2,IsUnique = true)]
        public string Name { get; set; }

        //Relacion
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Tipo de Documento")]
        [Index("CategoryDocument_Id_Name_Index", 1,IsUnique = true)]
        public int TypeDocumentId { get; set; }

        public virtual TypeDocument TypeDocument { get; set; }

        public virtual ICollection<DocumentEntry> DocumentEntries { get; set; }
    }
}