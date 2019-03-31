using Data.User;
using Logic.Parameters;

namespace Model
{
    public class ShopItem
    {
        public ReactiveParameter<int> Amount;
        
        public Data.ShopItem data { get; private set; }
        private UserShopItem _userShopItem;

        public ShopItem(UserShopItem userShopItem)
        {
            _userShopItem = userShopItem;
            data = App.Instance.catalog.ShopItems[_userShopItem.ItemId];
            
            Amount = new ReactiveParameter<int>(_userShopItem.Amount);
        }

        internal void Buy()
        {
            int amount = 1;
            if (App.Instance.FarmModel.ShopInventory.Buy(data, amount))
            {
                _userShopItem.Amount += amount;
                Amount.Value = _userShopItem.Amount;
                UserRepository.Save();
            }
        }
    }
}