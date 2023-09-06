using Microsoft.EntityFrameworkCore.Metadata.Internal;
using server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Order
    {
        [Key]
        public int Oder_id { get; set; }

        [ForeignKey("User")]
        public string Uid { get; set; }
        public User User { get; set; }

        [ForeignKey("Address")]
        public int Address_id { get; set; }
        public Address Address { get; set; }

        public string Created_at { get; set; }
        public string Updated_at { get; set; }
        public string Status { get; set; }
        public int Total { get; set; }
    }
}
