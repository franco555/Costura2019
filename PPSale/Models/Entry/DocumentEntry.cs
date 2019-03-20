using PPSale.Models.Departure;
using PPSale.Models.Globals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PPSale.Models.Entry
{
    public class DocumentEntry
    {
        [Key]
        public int DocumentEntryId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Tipo Documento")]
        public int TypeDocumentId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Categoría Doc.")]
        public int CategoryDocumentId { get; set; }


        [MaxLength(100, ErrorMessage = "El Campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Número Doc.")]
        public string NumberDoc { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        [Range(0, Double.MaxValue, ErrorMessage = "No tiene Precio {0}")]
        [Display(Name = "Importe Total")]
        public double PriceTotal { get; set; }

        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = false)]
        [Range(0, Double.MaxValue, ErrorMessage = "No tiene Iva {0}")]
        [Display(Name = "Iva")]
        public double Iva { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Proveedor")]
        public int ProviderId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Compañía")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Tipo de Pago")]
        public int TypePaymentId { get; set; }

        [Display(Name = "Guardado")]
        public bool save { get; set; }

        [Display(Name = "Pagado")]
        public bool Payment { get; set; }



        //Relaciones 
        public virtual Provider Provider { get; set; }
        public virtual Company Company { get; set; }
        public virtual TypeDocument TypeDocument { get; set; }
        public virtual CategoryDocument CategoryDocument { get; set; }
        public virtual TypePayment TypePayment { get; set; }

        //
        public virtual ICollection<DocumentEntryDetail> DocumentEntryDetails { get; set; }
    }
}