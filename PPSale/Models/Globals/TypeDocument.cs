
using PPSale.Models.Entry;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPSale.Models.Globals
{
    public class TypeDocument
    {
        [Key]
        public int TypeDocumentId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(50, ErrorMessage = "El Campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Tipo Doc.")]
        [Index("TypeDocument_Name_Index", IsUnique = true)]
        public string Name { get; set; }


        //Coleccion eviado a:
        public virtual ICollection<CategoryDocument> CategoryDocuments { get; set; }
        public virtual ICollection<DocumentEntry> DocumentEntries { get; set; }
    }
}