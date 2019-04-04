using Data.User;
using Logic.Parameters;

namespace Model
{
    public class Product : BaseModelItem<UserProduct>
    {
        public ReactiveParameter<int> Amount;
        
        public bool IsSellable
        {
            get
            {
                return Amount.Value > 0 && 
                       _userData.CatalogData.SellPrice > 0 && 
                       !string.IsNullOrEmpty(_userData.CatalogData.Currency);
            }
        }

        public Product(UserProduct userProduct) : base(userProduct)
        {
        }

        public void Sell()
        {
            if (!IsSellable)
                return;
            
            int amount = 1;
            if (App.Instance.FarmModel.ShopInventory.Sell(_userData.CatalogData))
            {
               Spend(amount);
            }
        }
        
        public void Spend(int amount = 1)
        {
            ChangeAmount(-amount);
        }

        public void ChangeAmount(int value)
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