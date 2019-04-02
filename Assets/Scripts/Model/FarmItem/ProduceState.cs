
using Logic.Parameters;


namespace Model
{

    public delegate void ProduceProductDelegate(string name, int amount);

    internal class ProduceState : FarmItemState
    {
        private Data.FarmItem _data;

        public event ProduceProductDelegate OnProduceComplete;

        private ReactiveParameter<float> _progress;
        private ReactiveParameter<float> _resourceTime;


        public ProduceState(Data.FarmItem data, ReactiveParameter<float> progress, ReactiveParameter<float> resourceTime) 
            :base(State.PRODUCE)
        {
            _data = data;
            _progress = progress;
            _resourceTime = resourceTime;
            _progress.OnValueChange += OnProgress ;
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
            _progress.Value += 1;
            _resourceTime.Value -= deltaTime;
        }
    }
}