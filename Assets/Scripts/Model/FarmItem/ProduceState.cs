
using Logic.Parameters;


namespace Model
{

    public delegate void ProduceProductDelegate(string name, int amount);

    internal class ProduceState : FarmItemState
    {
        private FarmItem _farmItem;
        public event ProduceProductDelegate OnProduceComplete;

        public ProduceState(FarmItem farmItem) :base(State.PRODUCE)
        {
            _farmItem = farmItem;
            _farmItem.Progress.OnValueChange += OnProgress ;
        }

        private void OnProgress (float oldvalue, float newvalue)
        {
            if (newvalue >= 1)
                _owner.SetState(State.IDLE);
        }

        public override void OnEnterState()
        {
            //throw new System.NotImplementedException();
        }

        public override void OnExitState()
        {
            
        }

        public override void Tick(float deltaTime)
        {
            _farmItem.Progress.Value += 1;
            _farmItem.ResourceTime.Value -= deltaTime;
        }

        public override void Release()
        {
            base.Release();
            _farmItem.Progress.OnValueChange -= OnProgress ;
        }
    }
}