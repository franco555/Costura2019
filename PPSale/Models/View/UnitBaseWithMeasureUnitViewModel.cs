namespace PPSale.Models.View
{
    using Complement;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class UnitBaseWithMeasureUnitViewModel
    {
        public int UnitBaseId { get; set; }
        
        [Display(Name = "Unidad de Medida Base")]
        public double Value { get; set; }
        
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        
        public ICollection<MeasuredUnit> measuredUnits { get; set; }
    }
}