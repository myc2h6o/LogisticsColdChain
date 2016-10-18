namespace ResourceManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Refrigerator_Car
    {
        [Key]
        [StringLength(255)]
        public string License_Plate_Number { get; set; }

        [StringLength(255)]
        public string Vehicle_Type { get; set; }

        public double? Vehicle_Load { get; set; }

        public int? Fuel_Consumption { get; set; }

        [StringLength(255)]
        public string Driver { get; set; }

        public double? Phone_Number { get; set; }

        [StringLength(255)]
        public string Minimum_Temperature { get; set; }

        public int? Maximum_Temperature { get; set; }

        [StringLength(255)]
        public string Company { get; set; }

        [StringLength(255)]
        public string Car_Register { get; set; }
    }
}
