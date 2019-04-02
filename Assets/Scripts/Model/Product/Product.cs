using Data.User;
using Logic.Parameters;

namespace Model
{
    
    
    public class Product
    {
        public enum State
        {
            Pending,
            Picked
        }
        
        public ReactiveParameter<int> Amount;
        
        public Data.Product data { get; private set; }

        public void OnPickUp()
        {
            Fsm.SetState(State.Picked);
        }
        
        private UserProduct _userProduct;
        
        public FSM<State, ProductState> Fsm { get; private set; }

        public Product(UserProduct userProduct, State state = State.Pending)
        {
            _userProduct = userProduct;
            Amount = new ReactiveParameter<int>(_userProduct.Amount);
            
            data = App.Instance.catalog.Products[_userProduct.ItemId];
            
            
        }
    }
}