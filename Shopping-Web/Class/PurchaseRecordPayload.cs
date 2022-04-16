using System;
using System.Collections.Generic;

namespace Shopping_Web.Class
{
    public class PurchaseRecordPayload
    {
        public string account { get; set; }
        public List<ShoppingItem> shoppingList { get; set; }
    }
}
