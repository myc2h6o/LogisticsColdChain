namespace ResourceManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Customer")]
    public partial class Customer
    {
        [Key]
        [StringLength(255)]
        public string Customer_Number { get; set; }

        [Column("Customer")]
        [StringLength(255)]
        public string Customer1 { get; set; }

        [StringLength(255)]
        public string Customer_Address { get; set; }

        public double? Phone_Number { get; set; }
    }
}
