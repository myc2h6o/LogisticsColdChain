namespace ResourceManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Orders
    {
        [Key]
        [StringLength(255)]
        public string Number { get; set; }

        [Required]
        [StringLength(255)]
        public string Supplier_Number { get; set; }

        [Required]
        [StringLength(255)]
        public string Customer_Number { get; set; }

        [Required]
        [StringLength(255)]
        public string Supplier_Cold_Storage_Number { get; set; }

        [Required]
        [StringLength(255)]
        public string Customer_Cold_Storage_Number { get; set; }

        [Required]
        [StringLength(255)]
        public string Commodity { get; set; }

        public int Quantity { get; set; }

        [Required]
        [StringLength(255)]
        public string Status { get; set; }

        [Required]
        [StringLength(255)]
        public string Car_Assigned { get; set; }

        [Required]
        [StringLength(255)]
        public string Deadline { get; set; }

        [Required]
        [StringLength(255)]
        public string Departure_Time { get; set; }

        [Required]
        [StringLength(255)]
        public string Arrival_Time { get; set; }
    }
}
