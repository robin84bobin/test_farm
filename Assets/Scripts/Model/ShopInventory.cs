using System.Collections.Generic;
using Logic.Parameters;

namespace Model
{
    public class ShopInventory
    {
        public ReactiveParameter<int> Coins;
        public Dictionary<string, Model.ShopItem> Items;

        public void Init()
        {
            int coinsValue = App.Instance.catalog.Currency["coins"].Value;
            Coins = new ReactiveParameter<int>(coinsValue);
            
            Items = new Dictionary<string, Model.ShopItem>();
            foreach (var shopItem in App.Instance.userRepository.ShopItems.GetAll())
            {
                Items.Add(shopItem.ItemId, new ShopItem(shopItem));
            }
        }
        
        public bool Buy(Data.ShopItem shopItemData)
        {
            //shopItemData.BuyPrice
            Coins.Value -= shopItemData.BuyPrice;
            return true;
        }
    }
}