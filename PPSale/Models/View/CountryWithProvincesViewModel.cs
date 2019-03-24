namespace PPSale.Models.View
{
    using Globals;
    using System.Collections.Generic;

    public class CountryWithProvincesViewModel
    {
        public int ContryId { get; set; }
        public string Name { get; set; }

        public ICollection<Province> provinces { get; set; }
    }
}