
namespace Model
{
    internal abstract class FarmItemState : BaseState<FarmItem.State>
    {
        public FarmItemState(FarmItem.State name) : base(name)
        {
        }

        public abstract void Tick(float deltaTime);
    }
}