namespace ResourceManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Vehicle_Status
    {
        [Key]
        [StringLength(255)]
        public string License_Plate_Number { get; set; }

        [Column("Vehicle_Status")]
        [StringLength(255)]
        public string Vehicle_Status1 { get; set; }

        [StringLength(255)]
        public string Setting_Temperature { get; set; }
        
        [StringLength(255)]
        public string Realtime_Temperature { get; set; }
        
        [StringLength(255)]
        public string Realtime_Position { get; set; }
    }
}
