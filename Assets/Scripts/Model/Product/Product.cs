using Data.User;
using Logic.Parameters;

namespace Model
{
    public class Product
    {
        public ReactiveParameter<int> Amount;
        
        public Data.Product data { get; private set; }

        public bool IsSellable
        {
            get
            {
                return Amount.Value > 0 && data.SellPrice > 0 && !string.IsNullOrEmpty(data.Currency);
            }
        }

        private UserProduct _userProduct;
        
        public Product(UserProduct userProduct)
        {
            _userProduct = userProduct;
            Amount = new ReactiveParameter<int>(_userProduct.Amount);
            
            data = App.Instance.catalog.Products[_userProduct.ItemId];
        }

        public void Sell()
        {
            if (!IsSellable)
                return;
            
            int amount = 1;
            if (App.Instance.FarmModel.ShopInventory.Sell(data))
            {
               ChangeAmount(-amount);
            }
        }

        public void ChangeAmount(int value)
        {
            _userProduct.Amount += value;
            Amount.Value = _userProduct.Amount;
            UserRepository.Save();
        }
    }
}