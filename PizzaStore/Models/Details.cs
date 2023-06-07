using PizzaStore.Areas.Identity.Data;

namespace PizzaStore.Models
{
    public class Details
    {
        public int Id { get; set; }
        public int quantity { get; set; }
        public DateTime ts { get; set; }
        public double price { get; set; }
        public Order Order { get; set; }
        public int OrderId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }

    }
}
