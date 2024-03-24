using FIMS2.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FIMS2.ViewModels
{

    public class LotsVM
    {
        [DisplayName("Lot Number")]
        [Required]
        public string LotNumber { get; set; }

        [Required]
        [DisplayName("Fluid Name")]
        public string LotName { get; set; }
        [DisplayName("Notes")]
        public string LotNotes { get; set; }

        [DisplayName("Total Quantity")]
        [Required]
        public int TotalQuantity { get; set; }
        [DisplayName("Available Quantity")]
        public int AvailableQuantity { get; set; }

        [DisplayName("Date Added")]
        public DateOnly DateOnly { get; set; }

       
    }
}

