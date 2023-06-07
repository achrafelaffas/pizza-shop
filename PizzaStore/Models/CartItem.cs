namespace PizzaStore.Models
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; } = 1;
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
