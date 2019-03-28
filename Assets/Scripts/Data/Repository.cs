using System;
using System.Collections.Generic;

namespace Data
{
    public class Repository
    {
        public Dictionary<string, Product> Products = new Dictionary<string, Product>();
        public Dictionary<string, FarmItem> FarmItems = new Dictionary<string, FarmItem>();
        public Dictionary<string, ShopItem> ShopItems = new Dictionary<string, ShopItem>();
    }
}