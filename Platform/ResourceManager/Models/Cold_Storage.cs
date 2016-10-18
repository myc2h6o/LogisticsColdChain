namespace ResourceManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Cold_Storage
    {
        [Key]
        [StringLength(255)]
        public string Cold_Storage_Number { get; set; }

        [StringLength(255)]
        public string Cold_Storage_Address { get; set; }

        [StringLength(255)]
        public string Cold_Storage_Scale { get; set; }

        [StringLength(255)]
        public string Cold_Storage_Company { get; set; }
    }
}
