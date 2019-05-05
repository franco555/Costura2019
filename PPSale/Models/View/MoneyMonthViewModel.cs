using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PPSale.Models.View
{
    public class MoneyMonthViewModel
    {
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
        public int ViewMoneyMonthId { get; set; }

        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public double Price { get; set; }
    }
}