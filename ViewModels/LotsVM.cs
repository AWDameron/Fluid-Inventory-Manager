using FIMS2.Models;
using System.ComponentModel.DataAnnotations;

namespace FIMS2.ViewModels
{

    public class LotsVM
    {
       
        [Required]
        public string LotNumber { get; set; }

        [Required]
        public string LotName { get; set; }
        public string LotNotes { get; set; }


        [Required]
        public int TotalQuantity { get; set; }
        public int AvailableQuantity { get; set; }

        public DateOnly DateOnly { get; set; }

       
    }
}

