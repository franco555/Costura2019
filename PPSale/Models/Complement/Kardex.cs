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

        [Display(Name = "Stock")]
        public double Stock { get; set; }

        [Display(Name = "P. Venta/Unidad")]
        public double Price { get; set; }

        /*****/
        
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Index("Kardex_Index", 2, IsUnique = true)]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Product Classification")]
        public int ProductClassificationId { get; set; }


        //Relaciones Simples
        public virtual ProductClassification ProductClassification { get; set; }
    }
}