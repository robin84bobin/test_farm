using Logic.Parameters;

namespace Model
{

    public class FarmItem : TickableItem, IProducer, IEater
    {
        public event ProduceProductDelegate OnProduceComplete;
        public ReactiveParameter<float> Progress;
        public ReactiveParameter<float> Resource;

        public enum State
        {
            IDLE,
            PRODUCE,
            
        }

        private Data.FarmItem _data;
        private FSM<State, FarmItemState> _fsm;


        public FarmItem(Data.FarmItem data)
        {
            _data = data;
            
            Progress = new ReactiveParameter<float>(0f);
            Resource = new ReactiveParameter<float>(0f);

            _fsm = new FSM<State, FarmItemState>();
            _fsm.Add(new IdleState());
            var produceState = new ProduceState(_data,Progress,Resource);
            produceState.OnProduceComplete += OnProduceComplete;
            _fsm.Add(produceState);
        }


        public bool Eat(IEatible food)
        {
            if (_data.ResourceProductId != food.Name)
                return false;

            StartProduce();
            return true;
        }

        private void StartProduce()
        {
            _fsm.SetState(State.PRODUCE);
        }

        public override void Release()
        {
            base.Release();
            _fsm.Release();
            OnProduceComplete = null;
        }

        protected override void OnTick()
        {
            _fsm.CurrentState.Tick();
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