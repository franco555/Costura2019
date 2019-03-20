using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PPSale.Models.Entry
{
    public class AddProductEntries
    {
        [Key]
        public int AddProductEntryId { get; set; }

        public int DocumentEntryId { get; set; }

        [Display(Name = "Fecha")]
        public DateTime Date { get; set; }

        [Display(Name = "Tipo Documento")]
        public string TypeDocument { get; set; }

        [Display(Name = "Categoría Doc.")]
        public string CategoryDocument { get; set; }

        [Display(Name = "Número Doc.")]
        public string NumberDoc { get; set; }

        [Display(Name = "Importe Total")]
        public double PriceTotal { get; set; }

        [Display(Name = "Total DocumentEntryDetail")]
        public double P_Total { get; set; }

        [Display(Name = "Total mas IVA")]
        public double P_Total_Iva { get; set; }

        [Display(Name = "Iva")]
        public double Iva { get; set; }

        [Display(Name = "Iva Total")]
        public double IvaTotal { get; set; }

        [Display(Name = "Proveedor")]
        public string p_Provider { get; set; }
        public string p_Address { get; set; }
        public string p_Cuit { get; set; }
        public string p_Email { get; set; }
        public string p_Logo { get; set; }
        public string p_IVA { get; set; }
        public string p_Date { get; set; }


        [Display(Name = "Compañía")]
        public string c_Company { get; set; }
        public string c_Address { get; set; }
        public string c_Cuit { get; set; }
        public string c_Email { get; set; }
        public string c_Logo { get; set; }
        public string c_IVA { get; set; }
        public string c_Date { get; set; }

        [Display(Name = "Tipo de Pago")]
        public string TypePayment { get; set; }

        [Display(Name = "Guardado")]
        public bool save { get; set; }

        [Display(Name = "Pagado")]
        public bool Payment { get; set; }

        

        public List<DocumentEntryDetail> DocumentEntryDetails { get; set; }
    }
}