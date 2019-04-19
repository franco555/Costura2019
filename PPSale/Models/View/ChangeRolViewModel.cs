using System;
using System.ComponentModel.DataAnnotations;

namespace PPSale.Models.View
{
    public class ChangeRolViewModel
    {
        [Key]
        public int ChangeRolViewModelId { get; set; }

        public int CompanyId { get; set; }

        public int UserId { get; set; }

        [Range(1, Double.MaxValue, ErrorMessage = "No ha selecionado {0}")]
        [Display(Name = "Rol")]
        public int RolId { get; set; }
    }
}