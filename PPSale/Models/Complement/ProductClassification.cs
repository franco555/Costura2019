
namespace PPSale.Models.Complement
{
    using Entry;
    using Globals;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public class ProductClassification
    {
        [Key]
        public int ProductClassificationId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(50, ErrorMessage = "El Campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Cod. Barra")]
        public string CodeBar { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Index("PC_PId_CId_CId_Index", 1, IsUnique = true)]
        [Display(Name = "Producto")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Index("PC_PId_CId_CId_Index", 2, IsUnique = true)]
        [Display(Name = "Clasificación")]
        public int ClassificationId { get; set; }

        [Index("PC_PId_CId_CId_Index", 4, IsUnique = true)]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Unidad Base")]
        public int UnitBaseId { get; set; }
        
        //[NotMapped]
        //public string ProductFull { get { return $"{Product.Name} [{ Classification.Name}]"; } }


        //Relaciones Simples
        public virtual Product Product { get; set; }
        public virtual Classification Classification { get; set; }
        public virtual Company Company { get; set; }
        public virtual UnitBase UnitBase { get; set; }

        //
        public virtual ICollection<Kardex> Kardexes { get; set; }
        public virtual ICollection<DocumentEntryDetail> DocumentEntryDetails { get; set; }
    }
}