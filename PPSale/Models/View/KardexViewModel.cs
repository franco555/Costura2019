
namespace PPSale.Models.View
{

    using System.ComponentModel.DataAnnotations;

    public class KardexViewModel
    {
        public int KardexId { get; set; }

        public int ProductClassificationId { get; set; }

        public int UnitBaseId { get; set; }

        [Display(Name = "Nombre Product")]
        public string ProductName { get; set; }

        [Display(Name = "Nombre Clasificación")]
        public string ClassificationName { get; set; }

        [Display(Name = "Nombre Unidad Base")]
        public string UnitBasenName { get; set; }

        [Display(Name = "Stock")]
        public double Stock { get; set; }

        [Display(Name = "Precio/Unidad")]
        public double Price { get; set; }

    }
}