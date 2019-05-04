using System.Collections.Generic;
using Data;
using Data.User;
using Logic.Parameters;

namespace Model
{
    public class ShopInventory
    {
        public Dictionary<string,ReactiveParameter<int>> Currencies;
        public Dictionary<string, Model.ShopItem> Items;

        public ShopInventory()
        {
            
        }
        
        public bool Buy(IBuyable item, int amount = 1)
        {
            int totalPrice = item.BuyPrice * amount;
            if (Currencies[item.Currency].Value < totalPrice)
                return false;
            
            SetCurrencyValue(item.Currency, Currencies[item.Currency].Value - totalPrice);
            return true;
        }
        
        public bool Sell(ISellable item, int amount = 1)
        {
            int totalPrice = item.SellPrice * amount;
            SetCurrencyValue(item.Currency, Currencies[item.Currency].Value + totalPrice);
            return true;
        }
        
        void SetCurrencyValue(string currencyId,int value)
        {
            Currencies[currencyId].Value = value;
            App.Instance.userRepository.Currency[currencyId].Value = value;
            UserRepository.Save();
        }

        public void Init()
        {
            int coinsValue = App.Instance.userRepository.Currency["coins"].Value;
            
            Currencies = new Dictionary<string, ReactiveParameter<int>>();
            foreach (var currency in App.Instance.userRepository.Currency)
            {
                Currencies.Add(currency.Id, new ReactiveParameter<int>(currency.Value));
            }
            
            Items = new Dictionary<string, Model.ShopItem>();
            foreach (var shopItem in App.Instance.userRepository.ShopItems.GetAll())
            {
                Items.Add(shopItem.Id, new ShopItem(shopItem));
            }
        }
    }
}