namespace ResourceManager.Models
{
    using System.Data.Entity;

    public partial class Logistics : DbContext
    {
        public Logistics()
            : base("name=DefaultConnection")
        {
        }

        public DbSet<Drivers> Drivers { get; set; }
    }
}
