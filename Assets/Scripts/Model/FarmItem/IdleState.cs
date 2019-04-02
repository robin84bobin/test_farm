namespace Model
{
    internal class IdleState : FarmItemState
    {
        public IdleState(FarmItem farmItem) : base(State.IDLE)
        {
            _farmItem = farmItem;
        }

        public override void OnEnterState()
        {
            _farmItem.TryStartProduce();
        }

        public override void OnExitState()
        {
            //throw new System.NotImplementedException();
        }

        public override void Tick(float deltaTime)
        {
            // new System.NotImplementedException();
        }
    }
}