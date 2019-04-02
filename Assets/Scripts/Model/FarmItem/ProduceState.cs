
using Logic.Parameters;

namespace Model
{

    public delegate void ProduceProductDelegate(string name, int amount);

    internal class ProduceState : FarmItemState
    {
        private Data.FarmItem _data;

        public event ProduceProductDelegate OnProduceComplete;


        public ProduceState(Data.FarmItem data, ReactiveParameter<float> progress, ReactiveParameter<float> resource) 
            :base(FarmItem.State.PRODUCE)
        {
            _data = data;
        }

        public override void OnEnterState()
        {
            //throw new System.NotImplementedException();
        }

        public override void OnExitState()
        {
            _owner.SetState(FarmItem.State.IDLE);
        }

        public override void Tick()
        {
            throw new System.NotImplementedException();
        }
    }
}