using Data.User;

namespace Model
{
    public class ShopItem
    {
        public int Amount;
        public Data.ShopItem data { get; private set; }
        private UserShopItem _userShopItem;
    }
}