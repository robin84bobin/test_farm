using System.Collections.Generic;
using Data.User;
using Logic.Parameters;

namespace Model
{
    public class ShopInventory
    {
        public ReactiveParameter<int> Coins;
        public Dictionary<string, Model.ShopItem> Items;

        public void Init()
        {
            int coinsValue = App.Instance.userRepository.Currency["coins"].Value;
            Coins = new ReactiveParameter<int>(coinsValue);
            
            Items = new Dictionary<string, Model.ShopItem>();
            foreach (var shopItem in App.Instance.userRepository.ShopItems.GetAll())
            {
                Items.Add(shopItem.ItemId, new ShopItem(shopItem));
            }
        }
        
        public bool Buy(Data.ShopItem shopItemData, int amount = 1)
        {
            int coins = shopItemData.BuyPrice * amount;
            
            if (Coins.Value < coins)
                return false;
            
            SetCoinsValue(Coins.Value - coins);
            return true;
        }

        void SetCoinsValue(int value)
        {
            Coins.Value = value;
            App.Instance.userRepository.Currency["coins"].Value = Coins.Value;
            UserRepository.Save();
        }
        
    }
}