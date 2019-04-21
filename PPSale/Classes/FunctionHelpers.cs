using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPSale.Classes
{
    public class FunctionHelpers
    {

        public string notUser { get; set; }
        public string notCompany { get; set; }
        public string notImage { get; set; }
        private string urlBase { get; set; }

        public FunctionHelpers()
        {
            urlBase = "~/Content/Logos/";

            notUser = $"{urlBase}NotUser.jpg";
            notCompany = $"{urlBase}NotCompany.jpg";
            notImage = $"{urlBase}NotImage.jpg";
        }
    }
}