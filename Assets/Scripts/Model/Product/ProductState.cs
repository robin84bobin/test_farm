using Model.FSM;

namespace Model
{
    public abstract class ProductState:BaseState<State>
    {
        public ProductState(State name) : base(name)
        {
        }
    }
}