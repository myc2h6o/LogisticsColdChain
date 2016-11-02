namespace ResourceManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Cold_Storage_Inventories
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Number { get; set; }

        public float Usage_Rate { get; set; }

        [Required]
        [StringLength(255)]
        public string Commodity { get; set; }

        public int Quantity { get; set; }
    }
}
