namespace ResourceManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Cars
    {
        [Key]
        [StringLength(255)]
        public string License_Plate_Number { get; set; }

        public float Tonnage { get; set; }

        public float Load_Bearing { get; set; }

        public int Fuel_Consumption { get; set; }

        [Required]
        [StringLength(255)]
        public string Driver { get; set; }

        [Required]
        [StringLength(255)]
        public string Phone_Number { get; set; }

        public float Minimum_Temperature { get; set; }

        public float Maximum_Temperature { get; set; }

        [Required]
        [StringLength(255)]
        public string Company { get; set; }

        [Required]
        [StringLength(255)]
        public string Register_Place { get; set; }
    }
}
