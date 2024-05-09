using System;

namespace ReactiveProperty
{
    public class ReactiveProperty<TValue> : IReadOnlyReactiveProperty<TValue>
    {
        private Action<TValue> _onValueChanged;
        private TValue _value;
        
        public TValue Value
        {
            get => _value;
            set
            {
                if (!_value.Equals(value))
                {
                    _value = value;
                    _onValueChanged?.Invoke(value);
                }
            }
            
        }

        public ReactiveProperty(TValue value = default)
        {
            _value = value;
        }

        public IDisposable SubscribeOnValueChanged(Action<TValue> onEvent, bool notifyOnSubscribe = false)
        {
            _onValueChanged += onEvent;
            
            if (notifyOnSubscribe)
                onEvent.Invoke(_value);
                
            return new DisposeAction(() =>
                _onValueChanged -= onEvent
            );
        }
    }
}