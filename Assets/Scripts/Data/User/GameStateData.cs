using System.Collections.Generic;

namespace Data.UserData
{
    public class GameStateData
    {
        public int money;
        
        public Dictionary<string, UserProductData> Products = new Dictionary<string, UserProductData>();
        public Dictionary<string, UserFarmItemData> FarmItems = new Dictionary<string, UserFarmItemData>();
        public Dictionary<string, UserShopItemData> ShopItems = new Dictionary<string, UserShopItemData>();
        
        
        public void Save()
        {
            
        }

        public void Load()
        {
            
        }
    }
}