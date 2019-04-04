namespace Model
{
    public class TickableItem
    {
        public void EnableTick()
        {
            App.Instance.OnTick += OnTick;
        }

        public virtual void Release()
        {
            App.Instance.OnTick -= OnTick;
        }

        protected virtual void OnTick(int deltaTime)
        {
        }
    }
}