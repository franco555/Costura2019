namespace PPSale.Models.View
{
    using PPSale.Models.Globals;
    using System.Collections.Generic;

    public class TDWithCDViewModel
    {
        public int TypeDocumentId { get; set; }

        public string Name { get; set; }

        public ICollection<CategoryDocument> categoryDocuments { get; set; }
    }
}