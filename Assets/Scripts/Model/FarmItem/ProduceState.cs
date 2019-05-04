
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

        public override void Tick(float deltaTime)
        {
            if (_farmItem.ResourceTime.Value > 0)
            {
                _farmItem.ResourceTime.Value -= deltaTime;
                _farmItem.Progress.Value += deltaTime;
            }
        }

    }
}