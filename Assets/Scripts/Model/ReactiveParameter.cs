namespace Logic.Parameters
{
    /// <summary>
    /// parameter that fire event on value change
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReactiveParameter<T> 
    {
        public event ValueChangeDelegate<T> OnValueChange = delegate { };

        public ReactiveParameter(T defaultValue = default(T))
        {
            SetDefaultValue(defaultValue);
        }

        public virtual void SetDefaultValue(T defaultValue)
        {
            _defaultValue = defaultValue;
        }

        protected T _defaultValue = default(T);
        protected T _value = default(T);
        
        public T Value
        {
            get { return _value; }
            set
            {
                T oldValue = _value;

                if (_value != null && _value.Equals(value))
                    return;

                _value = value;
                OnValueChange.Invoke(oldValue, _value);
            }
        }
    }


    public delegate void ValueChangeDelegate<T>(T oldValue, T newValue);
}