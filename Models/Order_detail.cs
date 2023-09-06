using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Order_detail
    {
        [Key]
        public int Order_items_id { get; set; }

        [ForeignKey("Order")]
        public int Order_id { get; set; }
        public Order Order { get; set; }


        [Required]
        [ForeignKey("Product")]
        public int Pid { get; set; }
        public Product Product { get; set; }

        public int quantity { get; set; }
        public int total { get; set; }
    }
}
