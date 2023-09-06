using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Address
    {
        [Key]
        public int Address_id { get; set; }

        [ForeignKey("User")]
        public string Uid { get; set; }
        public User User { get; set; }

        public string Ward { get; set; }
        public string Town { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
    }
}
