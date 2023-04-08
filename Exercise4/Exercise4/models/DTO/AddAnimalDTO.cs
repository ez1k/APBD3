using System.ComponentModel.DataAnnotations;

namespace Exercise4.models.DTO
{
    public class AddAnimalDTO
    {
        [Required]
        public int id { get; set; }

        [Required]
        [MaxLength(200)]
        public string name { get; set; } = String.Empty;
        [MaxLength(200)]
        public string description { get; set; } = String.Empty;
        [Required]
        [MaxLength(200)]
        public string category { get; set; } = String.Empty;
        [Required]
        [MaxLength(200)]
        public string area { get; set; } = String.Empty;

    }
}
