namespace PizzaStore.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public void AddItem(CartItem item)
        {
            // Check if the item already exists in the cart
            var existingItem = Items.FirstOrDefault(i => i.Product.Id == item.Product.Id);
            if (existingItem != null)
            {
                // Increment the quantity of the existing item
                existingItem.Quantity++;
            }
            else
            {
                // Add the item to the cart
                Items.Add(item);
            }
        }
    }
}
