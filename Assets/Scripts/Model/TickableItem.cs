namespace Model
{
    public class TickableItem
    {
        public TickableItem()
        {
            App.Instance.OnTick += OnTick;
        }

        public virtual void Release()
        {
            App.Instance.OnTick -= OnTick;
        }

        protected virtual void OnTick(float deltaTime)
        {
        }
    }
}