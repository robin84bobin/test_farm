namespace Model
{
    internal class IdleState : FarmItemState
    {
        public IdleState(FarmItem farmItem) : base(State.IDLE)
        {
            _farmItem = farmItem;
        }

        public override void Tick(float deltaTime)
        {
            //
        }

        public override void OnEnterState()
        {
            _farmItem.TryStartProduce();
        }

    }
}