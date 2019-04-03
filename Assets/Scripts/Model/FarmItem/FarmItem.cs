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

    public class FarmItem : BaseModelItem<UserFarmItem>
    {
        public event ProduceProductDelegate OnProduceComplete;
        public ReactiveParameter<float> Progress { get; private set; }
        public ReactiveParameter<int> ResourceTime { get; private set; }
        public ReactiveParameter<int> PendingCount { get; private set; }

        
        public FSM<State, FarmItemState> Fsm { get; private set; }

        private Queue<Data.Product> _pendingProducts = new Queue<Data.Product>();
        

        public FarmItem(UserFarmItem userFarmItem) : base(userFarmItem)
        {
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
                SaveData();
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
            
            SaveData();
        }

        public bool Eat(IEatible food)
        {
            if (_userData.CatalogData.ResourceProductId != food.Name)
                return false;

            ResourceTime.Value += _userData.CatalogData.ResourceTime;
            TryStartProduce();
            return true;
        }

        public void TryStartProduce()
        {
            //если не нужны ресурсы для производства - восполняем время
            if (string.IsNullOrEmpty(_userData.CatalogData.ResourceProductId))
                ResourceTime.Value = _userData.CatalogData.ResourceTime;
            
            if (ResourceTime.Value < _userData.CatalogData.ResourceTime)
                return;

            if (_pendingProducts.Count > 0)
                return;
            
            Fsm.SetState(State.PRODUCE);
            SaveData();
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

            Data.Product product = App.Instance.catalog.Products[_userData.CatalogData.ProductId];
            _pendingProducts.Enqueue(product);
            PendingCount.Value = _pendingProducts.Count;
            
            if (OnProduceComplete != null) 
                OnProduceComplete.Invoke(product.Id, _userData.CatalogData.ProduceAmount);

            Fsm.SetState(State.IDLE);
            
            SaveData();
        }

        protected override void InitData()
        {
            ResourceTime = new ReactiveParameter<int>(_userData.ResourceTime);
            Progress = new ReactiveParameter<float>(_userData.Progress);
            PendingCount = new ReactiveParameter<int>(_userData.PendingCount);
        }

        protected override void SaveData()
        {
            _userData.ResourceTime = ResourceTime.Value;
            _userData.Progress = Progress.Value;
            _userData.PendingCount = PendingCount.Value;
            UserRepository.Save();
        }
    }


    public interface IEatible
    {
        string Name { get; }
    }
}