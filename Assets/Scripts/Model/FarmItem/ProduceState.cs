
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

        private float deltaTime;
        
        public override void Tick(float tickTime)
        {
            if (_farmItem.ResourceTime.Value > 0)
            {
                deltaTime += tickTime;
                if (deltaTime < 1f)
                    return;
                
                _farmItem.ResourceTime.Value -= deltaTime;
                _farmItem.Progress.Value += deltaTime;
                deltaTime = 0f;
            }
        }

    }
}