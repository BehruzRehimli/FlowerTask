using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowersTask.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public int OrderSlider { get; set; }
        [MaxLength(100)]
        public string Image { get; set; }
        [Required]
        [MaxLength(30)]
        public string Title1 { get; set; }
        [Required]
        [MaxLength(30)]
        public string Title2 { get; set; }
        [Required]
        [MaxLength(100)]
        public string Desc { get; set; }
        [Required]
        [MaxLength(20)]
        public string Signature { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
