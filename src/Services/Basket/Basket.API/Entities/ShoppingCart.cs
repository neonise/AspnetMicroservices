using System.Collections.Generic;
using System.Linq;

namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {

        }

        public ShoppingCart(string username)
        {
            UserName = username;
        }

        public string UserName { get; set; }
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

        public decimal TotalPrice
        {
            get
            {
                if (!Items.Any())
                    return 0;

                return Items.Sum(x => x.Price * x.Quantity);
            }
        }
    }
}
