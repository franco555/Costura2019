namespace PPSale.Models.View
{
    using PPSale.Models.Globals;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CompanyWithUsersViewModel
    {
        public int CompanyId { get; set; }

        [Display(Name = "Compañia")]
        public string Name { get; set; }

        [Display(Name = "Direccion")]
        public string Address { get; set; }

        [Display(Name = "Logo")]
        public string Logo { get; set; }

        public ICollection<User> Users { get; set; }
    }
}