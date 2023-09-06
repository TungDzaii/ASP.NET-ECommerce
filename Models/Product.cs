using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Product
    {
        [Key]
        public int Pid { get; set; }

        [ForeignKey("Brand")]
        public int Bid { get; set; }
        public Brand Brand { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public int Stock { get; set; }

        public string Image { get; set; }

        public string Color { get; set; }

        public string Gift { get; set; }

        public string Model { get; set; }

        public string Warranty { get; set; }
        public string Description { get; set; }

        public DateTime Create_At { get; set; }
        public DateTime Update_At { get; set; }
        public int viewCount { get; set; }

        public Product()
        {
            viewCount = 0;
            Create_At = DateTime.Now;
            Update_At = DateTime.MinValue;
        }
    }
}
