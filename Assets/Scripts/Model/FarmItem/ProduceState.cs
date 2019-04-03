
using Logic.Parameters;


namespace Model
{

    public delegate void ProduceProductDelegate(string name, int amount);

    internal class ProduceState : FarmItemState
    {
        private FarmItem _farmItem;

        public ProduceState(FarmItem farmItem) :base(State.PRODUCE)
        {
            _farmItem = farmItem;
        }

        public override void Tick(int deltaTime)
        {
            _farmItem.Progress.Value += 1;
            _farmItem.ResourceTime.Value -= deltaTime;
        }

    }
}