namespace ResourceManager.Models
{
    using System.ComponentModel.DataAnnotations;

    public partial class Drivers
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Phone { get; set; }
    }
}
