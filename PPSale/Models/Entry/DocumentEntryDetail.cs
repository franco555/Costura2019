using PPSale.Models.Complement;
using PPSale.Models.Globals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPSale.Models.Entry
{
    public class DocumentEntryDetail
    {
        [Key]
        public int DocumentEntryDetailId { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Producto")]
        public int ProductId { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Clasificación")]
        public int ClassificationId { get; set; }

        public int ProductClassificationId { get; set; }

        [Display(Name = "Cantidad")]
        public double Quantity { get; set; }

        [Display(Name = "Precio")]
        public double Price { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Unidad")]
        public int MeasuredUnitId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Documento")]
        public int DocumentEntryId { get; set; }

        [Display(Name = "Compañia")]
        public int CompanyId { get; set; }

        [Display(Name = "Guardado")]
        public bool save { get; set; }

        [Display(Name = "Editado")]
        public bool edit { get; set; }

        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public double Value { get { return Price * Quantity; } }

        //[NotMapped]
        //public string Unit { get { return string.Format("{0} {1}", Quantity, MeasuredUnit.Name); } }

        [NotMapped]
        public string FullName { get ; set; }

        [NotMapped]
        [Display(Name = "Otro Producto")]
        public string OtherProduct { get; set; }

        //Relaciones 
        public virtual ProductClassification ProductClassification { get; set; }
        public virtual MeasuredUnit MeasuredUnit { get; set; }
        public virtual DocumentEntry DocumentEntry { get; set; }
        public virtual Company Company { get; set; }


        public virtual ICollection<TempDocEntryDetil> TempDocEntryDetils { get; set; }
    }
}