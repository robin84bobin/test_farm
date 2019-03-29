namespace Model
{
    public abstract class BaseState<TKey>
    {
        protected IStateMachine<TKey> _owner { get; private set; }
        public TKey Name { get; private set; }

        public abstract void OnEnterState();
        public abstract void OnExitState();

        public BaseState(TKey name)
        {
            Name = name;
        }

        public void SetOwner(IStateMachine<TKey> owner)
        {
            _owner = owner;
        }

        public virtual void Release()
        {
            _owner = null;
        }
    }

}


