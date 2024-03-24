using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIMS2.Models
{
    public class CustomerAllocation
    {
        [Key]
        public int AllocationID { get; set; }

        [Required]
        [DisplayName("Job Number")]
        public string CustomerNumber { get; set; }

        [Required]
        [DisplayName("Quantity Used")]
        public decimal QuantityUsed { get; set; }

        [ForeignKey(nameof(Lot))]
        [DisplayName("Fluid Lot Number")]
        public string LotNumber { get; set; }

        public Lot Lot { get; set; } 
    }
}
