
using Model.FSM;

namespace Model
{
    public abstract class FarmItemState : BaseState<State>
    {
        public FarmItemState(State name) : base(name)
        {
        }

        public abstract void Tick(float deltaTime);
    }
}