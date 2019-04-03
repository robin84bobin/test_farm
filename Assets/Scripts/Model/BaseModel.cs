using Data.User;

namespace Model
{
    public abstract class BaseModelItem<T> where T: UserDataItem<T>
    {
        protected abstract void InitData();
        protected abstract void SaveData();
    }
}