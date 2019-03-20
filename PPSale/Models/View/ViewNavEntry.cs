using PPSale.Models.Entry;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PPSale.Models.View
{
    public class ViewNavEntry
    {

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Desde")]
        public DateTime Date { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hasta")]
        public DateTime Date2 { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double TotalKilo { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public double TotalDinero { get; set; }

        public List<DocumentEntry> DocumentEntry { get; set; }
        public List<ViewProductQuantity> ProductQuantities { get; set; }
        public List<ViewMoneyMonth> MoneyMonths { get; set; }
    }
}