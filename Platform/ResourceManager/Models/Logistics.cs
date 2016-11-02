namespace ResourceManager.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Logistics : DbContext
    {
        public Logistics()
            : base("name=DefaultConnection")
        {
        }

        public virtual DbSet<Cars> Cars { get; set; }
        public virtual DbSet<Cars_Status> Cars_Status { get; set; }
        public virtual DbSet<Cold_Storage_Inventories> Cold_Storage_Inventories { get; set; }
        public virtual DbSet<Cold_Storages> Cold_Storages { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Suppliers> Suppliers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
