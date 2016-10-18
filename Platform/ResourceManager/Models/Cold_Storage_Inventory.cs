namespace ResourceManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Cold_Storage_Inventory
    {
        [Key]
        [StringLength(255)]
        public string Cold_Storage_Number { get; set; }

        public double? Inventory_Rate { get; set; }

        [StringLength(255)]
        public string Commodity { get; set; }

        public int? Quantity { get; set; }
    }
}
