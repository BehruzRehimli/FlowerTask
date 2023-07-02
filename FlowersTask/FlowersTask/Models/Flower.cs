using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowersTask.Models
{
    public class Flower
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(300)]
        public string Description { get; set; }
        [Required]
        [Column(TypeName ="decimal(6,2)")]
        public double Price { get; set; }
        public List<Image> Images { get; set; }
        public List<FlowerCatagory> FlowerCatagories { get; set; }
        [NotMapped]
        public IFormFile MainImage { get; set; }
        [NotMapped]
        public List<IFormFile> OtherImages { get; set; } = new List<IFormFile>();
        [NotMapped]
        public List<int> ImagesIds { get; set; }=new List<int>();
        [NotMapped]
        public List<int> CategoryIds { get; set; } = new List<int>();
    }
}
