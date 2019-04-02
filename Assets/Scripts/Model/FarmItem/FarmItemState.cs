
using Model.FSM;

namespace Model
{
    public abstract class FarmItemState : BaseState<State>
    {
        protected FarmItem _farmItem;
        
        public FarmItemState(State name) : base(name)
        {
        }

        public abstract void Tick(float deltaTime);

        public override void OnEnterState()
        {
            throw new System.NotImplementedException();
        }

        public override void OnExitState()
        {
            throw new System.NotImplementedException();
        }

        public override void Release()
        {
            base.Release();
            _farmItem = null;
        }
    }
}