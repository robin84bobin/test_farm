using Data.Repository;
using Data.User;
using Logic.Parameters;

namespace Model
{
    public class ShopItem : BaseModelItem<UserShopItem>
    {
        public ReactiveParameter<int> Amount;
        

        public ShopItem(UserShopItem userShopItem) : base(userShopItem)
        {
        }

        public void Spend(int amount = 1)
        {
            ChangeAmount(-amount);
        }

        internal void Buy()
        {
            int amount = 1;
            if (App.Instance.FarmModel.ShopInventory.Buy(_userData.CatalogData, amount))
            {
                ChangeAmount(amount);
            }
        }

        void ChangeAmount(int value)
        {
            Amount.Value += value;
            SaveData();
        }


        protected override void InitData()
        {
            Amount = new ReactiveParameter<int>(_userData.Amount);
        }

        protected override void SaveData()
        {
            _userData.Amount = Amount.Value;
            UserRepository.Save();
        }
    }
}