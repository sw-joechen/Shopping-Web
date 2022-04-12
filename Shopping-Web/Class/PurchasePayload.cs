using System.Collections.Generic;

namespace Shopping_Web.Class
{
    public class PurchasePayload
    {
        public string account { get; set; }
        public List<ShoppingItem> shoppingList { get; set; }
    }
}
