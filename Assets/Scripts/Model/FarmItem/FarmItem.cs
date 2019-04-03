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

        private Data.FarmItem _data;
        public FSM<State, FarmItemState> Fsm { get; private set; }

        private Queue<Data.Product> _pendingProducts = new Queue<Data.Product>();
        public int PendingCount
        {
            get { return _pendingProducts.Count; }
        }

        public FarmItem(Data.FarmItem data)
        {
            _data = data;
            
            ResourceTime = new ReactiveParameter<float>(0f);
            Progress = new ReactiveParameter<float>(0f);
            Progress.OnValueChange += OnProgress;
            
            var produceState = new ProduceState(this);
            Fsm = new FSM<State, FarmItemState>();
            Fsm.Add(produceState);
            Fsm.Add(new IdleState(this));
        }

        private void OnProgress(float oldvalue, float newvalue)
        {
            if (newvalue >= 1)
                Progress.Value = 0;

            Data.Product product = App.Instance.catalog.Products[_data.ProductId];
            _pendingProducts.Enqueue(product);
            
            if (OnProduceComplete != null) 
                OnProduceComplete.Invoke(product.Id, _data.ProduceAmount);

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
            if (!string.IsNullOrEmpty(_data.ResourceProductId) && _data.ResourceProductId != food.Name)
                return false;

            ResourceTime.Value += _data.ResourceTime;
            TryStartProduce();
            return true;
        }

        public void TryStartProduce()
        {
            if (ResourceTime.Value < _data.ResourceTime)
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