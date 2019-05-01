using System.ComponentModel.DataAnnotations;

namespace PPSale.Models.View
{
    public class ProductClassificationAndKardex
    {
        [Display(Name = "Producto")]
        public int ProductId { get; set; }
        
        [Display(Name = "Clasificación")]
        public int ClassificationId { get; set; }

        [Display(Name = "Compañia")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Unidad Base")]
        public int UnitBaseId { get; set; }
        
        [Display(Name = "Cod. Barra")]
        public string CodeBar { get; set; }

        [Display(Name = "Stock")]
        public double Stock { get; set; }

        [Display(Name = "P. Venta/Unidad")]
        public double Price { get; set; }

    }
}