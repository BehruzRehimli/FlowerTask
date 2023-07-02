namespace FlowersTask.Models
{
    public class FlowerCatagory
    {
        public int FlowerId { get; set; }
        public int CatagoryId { get; set; }

        public Catagory Catagory { get; set; }
        public Flower Flower { get; set; }

    }
}
