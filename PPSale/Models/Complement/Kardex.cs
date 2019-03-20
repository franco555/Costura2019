using PPSale.Models.Globals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PPSale.Models.Complement
{
    public class Kardex
    {

        [Key]
        public int KardexId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(50, ErrorMessage = "El Campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Cod. Barra")]
        public string CodeBar { get; set; }

        [Display(Name = "Stock")]
        public double Stock { get; set; }

        

        [Display(Name = "P. Venta/Unidad")]
        public double Price { get; set; }


        /*****/

        [Index("Kardex_Index", 1, IsUnique = true)]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Index("Kardex_Index", 3, IsUnique = true)]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Unidad")]
        public int MeasuredUnitId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Index("Kardex_Index", 2, IsUnique = true)]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Product Classification")]
        public int ProductClassificationId { get; set; }


        //Relaciones Simples
        public virtual MeasuredUnit MeasuredUnit { get; set; }
        public virtual Company Company { get; set; }
        public virtual ProductClassification ProductClassification { get; set; }
    }
}