using System.ComponentModel.DataAnnotations;

namespace FlowersTask.Models
{
    public class Catagory
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public List<FlowerCatagory> FlowerCatagories { get; set; }

    }
}
