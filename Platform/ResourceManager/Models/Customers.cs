namespace ResourceManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Customers
    {
        [Key]
        [StringLength(255)]
        public string Number { get; set; }

        [Required]
        [StringLength(255)]
        public string CName { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [StringLength(255)]
        public string Phone_Number { get; set; }
    }
}
