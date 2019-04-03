using System.Collections.Generic;
using System.Linq;
using Logic.Parameters;

namespace Model
{
    public enum State
    {
        IDLE,
        PRODUCE,
    }

    public class FarmItem : TickableItem
    {
        public event ProduceProductDelegate OnProduceComplete;
        public ReactiveParameter<float> Progress { get; private set; }
        public ReactiveParameter<float> ResourceTime { get; private set; }

        public Data.FarmItem Data { get; private set; }
        public FSM<State, FarmItemState> Fsm { get; private set; }

        private Queue<Data.Product> _pendingProducts = new Queue<Data.Product>();
        public int PendingCount
        {
            get { return _pendingProducts.Count; }
        }

        public FarmItem(Data.FarmItem data)
        {
            Data = data;
            
            ResourceTime = new ReactiveParameter<float>(0f);
            Progress = new ReactiveParameter<float>(0f);

            var produceState = new ProduceState(this);
            Fsm = new FSM<State, FarmItemState>();
            Fsm.Add(produceState);
            Fsm.Add(new IdleState(this));
        }

        public void OnInitInCell()
        {
            Progress.OnValueChange += OnProgress;
            Fsm.SetState(State.IDLE);
        }

        private void OnProgress(float oldvalue, float newvalue)
        {
            if (newvalue >= 1)
                Progress.Value = 0;

            Data.Product product = App.Instance.catalog.Products[Data.ProductId];
            _pendingProducts.Enqueue(product);
            
            if (OnProduceComplete != null) 
                OnProduceComplete.Invoke(product.Id, Data.ProduceAmount);

            Fsm.SetState(State.IDLE);
        }

        public bool PickUp(out Data.Product product)
        {
            product = _pendingProducts.Dequeue();
            if (_pendingProducts.Count <= 0)
                TryStartProduce();
            return product != null;
        }

        public bool Eat(IEatible food)
        {
            if (Data.ResourceProductId != food.Name)
                return false;

            ResourceTime.Value += Data.ResourceTime;
            TryStartProduce();
            return true;
        }

        public void TryStartProduce()
        {
            if (ResourceTime.Value < Data.ResourceTime)
                return;

            if (_pendingProducts.Count > 0)
                return;
            
            Fsm.SetState(State.PRODUCE);
        }

        public override void Release()
        {
            base.Release();
            Fsm.Release();
            OnProduceComplete = null;
        }

        protected override void OnTick(float deltaTime)
        {
            Fsm.CurrentState.Tick(deltaTime);
        }

        public void ProduceComplete()
        {
            
        }
    }


    public interface IEatible
    {
        string Name { get; }
    }
}