using System;
using TMPro;

namespace Model
{
    public class FarmItem : IProducer, IEater
    {
        public event ProduceProductDelegate OnProduceComplete;
        private Data.FarmItem _data;

        public FarmItem(Data.FarmItem data)
        {
            _data = data;
        }
        

        public bool Eat(IEatible food)
        {
            if (_data.resource != food.Name)
                return false;

            StartProduce();
            return true;
        }

        private void StartProduce()
        {
            //TODO wait period
            
            
            
            if (OnProduceComplete != null)
                OnProduceComplete.Invoke(_data.product,1);
        }

        public void Release()
        {
            OnProduceComplete = null;
        }
    }

    public delegate void ProduceProductDelegate(string name, int amount);
    
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