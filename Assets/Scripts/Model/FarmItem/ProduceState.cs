
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
            :base(FarmItem.State.PRODUCE)
        {
            _data = data;
            _progress = progress;
            _resourceTime = resourceTime;
        }

        public override void OnEnterState()
        {
            //throw new System.NotImplementedException();
        }

        public override void OnExitState()
        {
            _owner.SetState(FarmItem.State.IDLE);
        }

        public override void Tick(float deltaTime)
        {
            _progress.Value += 1;
        }
    }
}