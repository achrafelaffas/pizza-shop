using PizzaStore.Areas.Identity.Data;

namespace PizzaStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public String OrderNumber { get; set; } = String.Empty;
        public DateTime CreatedDate { get; set; }
        public String OrderStatus { get; set; } = string.Empty;
        public double Total { get; set; }
        public PizzaStoreUser PizzaStoreUser { get; set; }  
        public string UserId { get; set; }
    }
}
