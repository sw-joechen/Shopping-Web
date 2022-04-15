using System.Collections.Generic;

namespace Shopping_Web.Class
{
    public class PurchaseHistory
    {
        public string orderNumber { get; set; }
        public string account { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string createdDate { get; set; }
        public List<ShoppingItem> shoppingList { get; set; }
    }
}
