namespace ResourceManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Supplier")]
    public partial class Supplier
    {
        [Key]
        [StringLength(255)]
        public string Supplier_Number { get; set; }

        [Column("Supplier")]
        [StringLength(255)]
        public string Supplier1 { get; set; }

        [StringLength(255)]
        public string Supplier_Address { get; set; }

        public double? Phone_Number { get; set; }
    }
}
