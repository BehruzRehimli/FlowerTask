using System.ComponentModel.DataAnnotations;

namespace FlowersTask.Models
{
    public class Image
    {
        public int Id { get; set; }
        public int FlowerId { get; set; }
        [Required]
        [MaxLength(150)]
        public string ImageName { get; set; }
        [Required]
        public bool IsMain { get; set; } = false;
        public Flower Flower { get; set; }
    }
}
