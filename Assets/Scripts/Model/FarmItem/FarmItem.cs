using System.Collections.Generic;
using Data.User;
using Logic.Parameters;
using UnityEngine;
using Zenject;

namespace Model
{
    public enum State
    {
        IDLE,
        PRODUCE,
    }

    public class FarmItem : BaseModelItem<UserFarmItem>, IFixedTickable
    {
        public event ProduceProductDelegate OnProduceComplete;
        public ReactiveParameter<float> Progress { get; private set; }
        public ReactiveParameter<float> ResourceTime { get; private set; }
        public ReactiveParameter<int> PendingCount { get; private set; }
        public float ResourceMax{ get; private set; }
        
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
            ResourceTime.OnValueChange += OnResourceChanged;
            Fsm.SetState(State.IDLE);
            
        }

        private void OnResourceChanged(float oldvalue, float newvalue)
        {
            Debug.Log("OnResourceChanged:" + newvalue);
            if (newvalue <= 0)
            {
                ResourceTime.Value = 0;
            }
        }

        private void OnProgress(float oldvalue, float newvalue)
        {
            Debug.Log("OnProgress:" + newvalue);
            if (newvalue >= _userData.CatalogData.ProduceDuration)
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

        public bool Eat(Data.Product product)
        {
            if (_userData.CatalogData.ResourceProductId != product.Id)
                return false;

            App.Instance.FarmModel.ProductInventory.Items[product.Id].Spend();
            ResourceTime.Value += _userData.CatalogData.ResourceTime;
            ResourceMax = ResourceTime.Value;
            TryStartProduce();
            return true;
        }

        public void TryStartProduce()
        {
            //если не нужны ресурсы для производства - восполняем время
            if (string.IsNullOrEmpty(_userData.CatalogData.ResourceProductId))
                ResourceTime.Value = _userData.CatalogData.ProduceDuration;
            
            if (!IsEnoughResources)
                return;

            if (_pendingProducts.Count > 0)
                return;
            
            Fsm.SetState(State.PRODUCE);
            SaveData();
        }

        public bool IsEnoughResources
        {
            get { return ResourceTime.Value >= _userData.CatalogData.ProduceDuration; }
        }

        public void Release()
        {
            Fsm.Release();
            OnProduceComplete = null;
        }

        protected  void OnTick(int deltaTime)
        {
            
        }

        public void ProduceComplete()
        {
            Progress.Value = 0;

            var product = CreateProduct();
            PendingCount.Value = _pendingProducts.Count;
            
            if (OnProduceComplete != null) 
                OnProduceComplete.Invoke(product.Id, _userData.CatalogData.ProduceAmount);

            Fsm.SetState(State.IDLE);
            
            SaveData();
        }

        private Data.Product CreateProduct()
        {
            Data.Product product = App.Instance.catalog.Products[_userData.CatalogData.ProductId];
            _pendingProducts.Enqueue(product);
            return product;
        }

        protected override void InitData()
        {
            ResourceTime = new ReactiveParameter<float>(_userData.ResourceTime);
            Progress = new ReactiveParameter<float>(_userData.Progress);
            PendingCount = new ReactiveParameter<int>(_userData.PendingCount);

            ResourceMax = ResourceTime.Value;

            for (int i = 0; i < PendingCount.Value; i++)
                CreateProduct();
        }

        protected override void SaveData()
        {
            _userData.ResourceTime = ResourceTime.Value;
            _userData.Progress = Progress.Value;
            _userData.PendingCount = PendingCount.Value;
            UserRepository.Save();
        }

        public void FixedTick()
        {
            Fsm.CurrentState.Tick(Time.fixedDeltaTime);
        }
    }


}