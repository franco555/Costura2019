using PPSale.Models.Complement;
using System;
using System.ComponentModel.DataAnnotations;

namespace PPSale.Models.Entry
{
    public class TempDocEntryDetil
    {
        [Key]
        public int TempDocEntryDetilId { get; set; }

        public int DocumentEntryDetailId { get; set; }

        public int ProductClassificationId { get; set; }

        public double Quantity { get; set; }

        public double Price { get; set; }

        public bool State { get; set; }

        public DateTime Date { get; set; }

        public int MeasuredUnitId { get; set; }


        //**********************
        public virtual MeasuredUnit MeasuredUnit { get; set; }
        public virtual ProductClassification ProductClassification { get; set; }
        public virtual DocumentEntryDetail DocumentEntryDetail { get; set; }
    }
}