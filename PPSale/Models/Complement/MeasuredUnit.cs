using PPSale.Models.Entry;
using PPSale.Models.Globals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PPSale.Models.Complement
{
    public class MeasuredUnit
    {
        [Key]
        public int MeasuredUnitId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(50, ErrorMessage = "El Campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Unidad de Medida")]
        [Index("MeasuredUnit_NameCompanyId_Index", 2,IsUnique = true)]
        public string Name { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(50, ErrorMessage = "El Campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Abreviatura")]
        public string Abrev { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Valor")]
        public double Value { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Equivalencia")]
        public double Equivalent { get; set; }

        //Relacion
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Compañia")]
        [Index("MeasuredUnit_NameCompanyId_Index", 1,IsUnique = true)]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Unidad Base")]
        public int UnitBaseId { get; set; }

        //
        public virtual Company Company { get; set; }
        public virtual UnitBase UnitBase { get; set; }

        //
        public virtual ICollection<ProductClassification> ProductClassifications { get; set; }
        public virtual ICollection<DocumentEntryDetail> DocumentEntryDetails { get; set; }
        public virtual ICollection<TempDocEntryDetil> TempDocEntryDetils { get; set; }
    }
}