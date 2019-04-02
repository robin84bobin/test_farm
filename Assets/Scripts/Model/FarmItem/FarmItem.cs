using Logic.Parameters;

namespace Model
{
    public enum State
    {
        IDLE,
        PRODUCE,
    }

    public class FarmItem : TickableItem, IProducer, IEater
    {
        public event ProduceProductDelegate OnProduceComplete;
        public ReactiveParameter<float> Progress;
        public ReactiveParameter<float> ResourceTime;

        private Data.FarmItem _data;
        public FSM<State, FarmItemState> Fsm { get; private set; }

        public bool PendingProduct{ get; private set; }

        public FarmItem(Data.FarmItem data)
        {
            _data = data;
            
            Progress = new ReactiveParameter<float>(0f);
            Progress.OnValueChange += OnProgress;
            
            ResourceTime = new ReactiveParameter<float>(0f);

            Fsm = new FSM<State, FarmItemState>();
            Fsm.Add(new IdleState());
            var produceState = new ProduceState(_data,Progress,ResourceTime);
            produceState.OnProduceComplete += OnProduceComplete;
            Fsm.Add(produceState);
        }

        private void OnProgress(float oldvalue, float newvalue)
        {
            if (newvalue >= 1)
                Progress.Value = 0;
            
            Fsm.SetState(State.IDLE);
        }

        public void PickUp()
        {
            PendingProduct = false;
            StartProduce();
        }

        public bool Eat(IEatible food)
        {
            if (_data.ResourceProductId != food.Name)
                return false;

            ResourceTime.Value += _data.ResourceTime;
            StartProduce();
            return true;
        }

        private void StartProduce()
        {
            if (ResourceTime.Value < _data.ResourceTime)
                return;

            if (PendingProduct)
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
    }


    
    public interface IProducer
    {
        event ProduceProductDelegate OnProduceComplete;
    }

    public interface IEater
    {
        bool Eat(IEatible food);
    }

    public interface IEatible
    {
        string Name { get; }
    }
}