using System;

namespace ReactiveProperty
{
    public interface IReadOnlyReactiveProperty<out TValue>
    {
        public TValue Value { get; }
        public IDisposable SubscribeOnValueChanged(Action<TValue> onEvent, bool notifyOnSubscribe = false);
    }
}