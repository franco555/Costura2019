

namespace PPSale.Models.View
{
    using Complement;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ClassificationWithProductsViewModel
    {
        public int ClassificationId { get; set; }
        
        [Display(Name = "Clasificación")]
        public string Name { get; set; }

        public ICollection<Product> ProductOn { get; set; }

        public ICollection<Product> ProductOff { get; set; }
    }
}