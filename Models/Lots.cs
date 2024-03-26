using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FIMS2.Models
{
    public class Lot
    {
        [Key]
        [Required]
        [DisplayName("Lot Number")]
        public string LotNumber { get; set; }

        [Required]
        [DisplayName("Fluid Name")]
        public string LotName { get; set; }
       
        [DisplayName("Notes")]
        public string LotNotes { get; set; }
        
        
        [Required]
        [DisplayName("Total Quantity")]
        public decimal TotalQuantity { get; set; }
        [DisplayName("Available Quantity")]
        public decimal AvailableQuantity { get; set; }

        [DisplayName("Date Added")]
        public DateOnly DateOnly { get; set; }

        [DisplayName("Is Active?")]
        public bool IsActive { get; set; } = true;

    }
}
