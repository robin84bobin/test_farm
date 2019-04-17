using Data;

namespace Model
{
    public abstract class BaseModelItem<T> :TickableItem where T: DataItem
    {
        protected T _userData;
        public T UserData
        {
            get { return _userData; }
        }
        public BaseModelItem(T userData)
        {
            _userData = userData;
            InitData();
        }
        
        protected abstract void InitData();
        protected abstract void SaveData();
    }
}