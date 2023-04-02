using System.ComponentModel.DataAnnotations;

namespace Exercise4.models.DTO
{
    public class AddAnimalDTO
    {
        [Required]
        public int id { get; set; }

        [Required]
        public string name { get; set; } = String.Empty;
        [Required]
        public string description { get; set; } = String.Empty;
        [Required]
        public string category { get; set; } = String.Empty;
        [Required]
        public string area { get; set; } = String.Empty;

    }
}
