namespace ResourceManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Distribution")]
    public partial class Distribution
    {
        [Key]
        [StringLength(255)]
        public string Distribution_Number { get; set; }

        [StringLength(255)]
        public string Supplier_Number { get; set; }

        [StringLength(255)]
        public string Customer_Number { get; set; }

        [StringLength(255)]
        public string Supplier_Cold_Storage_Number { get; set; }

        [StringLength(255)]
        public string Customer_Cold_Storage_Number { get; set; }

        [StringLength(255)]
        public string Commodity { get; set; }

        [StringLength(255)]
        public string Quantity { get; set; }

        [StringLength(255)]
        public string Distribution_Status { get; set; }

        [StringLength(255)]
        public string Distribution_Vehicle { get; set; }

        [StringLength(255)]
        public string Deadline { get; set; }

        [StringLength(255)]
        public string Departure_Time { get; set; }

        [StringLength(255)]
        public string Arrival_Time { get; set; }
    }
}
