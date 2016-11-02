namespace ResourceManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Cars_Status
    {
        [Key]
        [StringLength(255)]
        public string License_Plate_Number { get; set; }

        [Required]
        [StringLength(255)]
        public string Status { get; set; }

        public float? Setting_Temperature { get; set; }

        public float? Realtime_Temperature { get; set; }

        [Required]
        [StringLength(255)]
        public string Realtime_Position { get; set; }
    }
}
