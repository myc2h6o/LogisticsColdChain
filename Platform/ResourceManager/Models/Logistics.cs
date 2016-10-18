namespace ResourceManager.Models
{
    using System.Data.Entity;

    public partial class Logistics : DbContext
    {
        public Logistics()
            : base("name=DefaultConnection")
        {
        }

        public DbSet<Cold_Storage> Cold_Storage { get; set; }
        public DbSet<Cold_Storage_Inventory> Cold_Storage_Inventory { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Distribution> Distribution { get; set; }
        public DbSet<Refrigerator_Car> Refrigerator_Car { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Vehicle_Status> Vehicle_Status { get; set; }
    }
}
