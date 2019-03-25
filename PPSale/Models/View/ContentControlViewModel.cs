namespace PPSale.Models.View
{

    using PPSale.Models.Globals;
    using System.Collections.Generic;

    public class ContentControlViewModel
    {
        //Iva,  Tipo de documento,  Tipo de pago,   

        public ICollection<IvaCondition> ivaConditions { get; set; }
        public ICollection<TypePayment> typePayments { get; set; }
        public ICollection<TypeDocument> typeDocuments { get; set; }
    }
}