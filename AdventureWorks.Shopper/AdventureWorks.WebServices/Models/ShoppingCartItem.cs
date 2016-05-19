

namespace AdventureWorks.WebServices.Models
{
    public class ShoppingCartItem
    {
        public string Id { get; set; }

        public Product Product{ get; set;}

        public int Quantity { get; set; }

        public string Currency { get; set; }
    }
}
