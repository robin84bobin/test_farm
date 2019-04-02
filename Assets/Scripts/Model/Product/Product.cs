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
    }
}