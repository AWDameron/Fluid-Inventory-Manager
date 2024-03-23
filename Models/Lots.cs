using System.ComponentModel.DataAnnotations;

namespace FIMS2.Models
{
    public class Lot
    {
        [Key]
        [Required]
        public string LotNumber { get; set; }

        [Required]
        public string LotName { get; set; }
        public string LotNotes { get; set; }
        
        
        [Required]
        public decimal TotalQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }

        public DateOnly DateOnly { get; set; }

        public bool IsActive { get; set; } = true;

    }
}
