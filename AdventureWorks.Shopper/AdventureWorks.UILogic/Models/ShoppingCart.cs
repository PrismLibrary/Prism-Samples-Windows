using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AdventureWorks.UILogic.Models
{
    [DataContract]
    public class ShoppingCart
    {
        public ShoppingCart(ICollection<ShoppingCartItem> shoppingCartItems)
        {
            ShoppingCartItems = shoppingCartItems;
        }

        [DataMember]
        public ICollection<ShoppingCartItem> ShoppingCartItems { get; private set; }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public double FullPrice { get; set; }

        [DataMember]
        public double TotalDiscount { get; set; }

        [DataMember]
        public double TotalPrice { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public double TaxRate { get; set; }
    }
}
