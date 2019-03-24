namespace PPSale.Models.View
{
    using Globals;
    using System.Collections.Generic;
    public class ProvinceWithCititesViewModel
    {
        public int ContryId { get; set; }
        public int ProvinceId { get; set; }
        public string Name { get; set; }

        public ICollection<City> cities{ get; set; }
    }
}
