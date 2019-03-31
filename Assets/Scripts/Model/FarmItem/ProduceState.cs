
namespace Model
{

    public delegate void ProduceProductDelegate(string name, int amount);

    internal class ProduceState : FarmItemState
    {
        private Data.FarmItem _data;

        public event ProduceProductDelegate OnProduceComplete;

        public ProduceState(Data.FarmItem data) : base(FarmItem.State.PRODUCE)
        {
            _data = data;
        }

        public override void OnEnterState()
        {
            throw new System.NotImplementedException();
        }

        public override void OnExitState()
        {
            throw new System.NotImplementedException();
        }

        public override void Tick()
        {
            throw new System.NotImplementedException();
        }
    }
}