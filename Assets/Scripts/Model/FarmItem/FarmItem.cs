using System.Collections.Generic;
using System.Linq;
using Data.User;
using Logic.Parameters;
using TMPro;

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
        public ReactiveParameter<int> ResourceTime { get; private set; }
        public ReactiveParameter<int> PendingCount { get; private set; }

        public UserFarmItem userFarmItem { get; private set; }
        private Data.FarmItem _data;
        
        public FSM<State, FarmItemState> Fsm { get; private set; }

        private Queue<Data.Product> _pendingProducts = new Queue<Data.Product>();
        

        public FarmItem(UserFarmItem userFarmItem)
        {
            this.userFarmItem = userFarmItem;
            if (!string.IsNullOrEmpty(userFarmItem.ItemId))
                _data = App.Instance.catalog.FarmItems[userFarmItem.ItemId];
            
            ResourceTime = new ReactiveParameter<int>(userFarmItem.ResourceTime);
            Progress = new ReactiveParameter<float>(userFarmItem.Progress);
            PendingCount = new ReactiveParameter<int>(userFarmItem.PendingCount);

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
                ProduceComplete();
            else
            {
                Save();
            }
        }

        public void PickUp()
        {
            if (_pendingProducts.Count <= 0)
                return;
            
            Data.Product product = _pendingProducts.Dequeue();
            if (_pendingProducts.Count <= 0)
                TryStartProduce();

            PendingCount.Value = _pendingProducts.Count;

            App.Instance.FarmModel.ProductInventory.Add(product);
            
            Save();
        }

        public bool Eat(IEatible food)
        {
            if (_data.ResourceProductId != food.Name)
                return false;

            ResourceTime.Value += _data.ResourceTime;
            TryStartProduce();
            return true;
        }

        public void TryStartProduce()
        {
            //если не нужны ресурсы для производства - восполняем время
            if (string.IsNullOrEmpty(_data.ResourceProductId))
                ResourceTime.Value = _data.ResourceTime;
            
            if (ResourceTime.Value < _data.ResourceTime)
                return;

            if (_pendingProducts.Count > 0)
                return;
            
            Fsm.SetState(State.PRODUCE);
            Save();
        }

        public override void Release()
        {
            base.Release();
            Fsm.Release();
            OnProduceComplete = null;
        }

        protected override void OnTick(int deltaTime)
        {
            Fsm.CurrentState.Tick(deltaTime);
        }

        public void ProduceComplete()
        {
            Progress.Value = 0;

            Data.Product product = App.Instance.catalog.Products[_data.ProductId];
            _pendingProducts.Enqueue(product);
            PendingCount.Value = _pendingProducts.Count;
            
            if (OnProduceComplete != null) 
                OnProduceComplete.Invoke(product.Id, _data.ProduceAmount);

            Fsm.SetState(State.IDLE);
            
            Save();
        }

        void Save()
        {
            userFarmItem.ResourceTime = ResourceTime.Value;
            userFarmItem.Progress = Progress.Value;
            userFarmItem.PendingCount = PendingCount.Value;
            UserRepository.Save();
        }
    }


    public interface IEatible
    {
        string Name { get; }
    }
}