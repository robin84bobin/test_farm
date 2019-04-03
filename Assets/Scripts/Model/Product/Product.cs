using Data.User;
using Logic.Parameters;

namespace Model
{
    public class Product
    {
        public ReactiveParameter<int> Amount;
        
        public Data.Product data { get; private set; }

        public void OnPickUp()
        {
            Amount.Value ++;
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
            int amount = 1;
            if (App.Instance.FarmModel.ShopInventory.Sell(data))
            {
                _userProduct.Amount += amount;
                Amount.Value = _userProduct.Amount;
                UserRepository.Save();
            }
        }
    }
}