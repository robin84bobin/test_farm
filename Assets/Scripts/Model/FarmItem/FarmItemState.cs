
using Model.FSM;

namespace Model
{
    public abstract class FarmItemState : BaseState<State>
    {
        protected FarmItem _farmItem;
        
        public FarmItemState(State name) : base(name)
        {
        }

        public abstract void Tick(int deltaTime);

        public override void OnEnterState()
        {
            //
        }

        public override void OnExitState()
        {
            //
        }

        public override void Release()
        {
            base.Release();
            _farmItem = null;
        }
    }
}