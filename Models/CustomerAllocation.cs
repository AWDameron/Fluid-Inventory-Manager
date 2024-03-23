using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIMS2.Models
{
    public class CustomerAllocation
    {
        [Key]
        public int AllocationID { get; set; }

        [Required]
        public string CustomerNumber { get; set; }

        [Required]
        public decimal QuantityUsed { get; set; }

        [ForeignKey(nameof(Lot))]
        public string LotNumber { get; set; }

        public Lot Lot { get; set; } 
    }
}
