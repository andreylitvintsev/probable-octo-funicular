using System;

namespace ReactiveProperty
{
    public class DisposeAction : IDisposable
    {
        private readonly Action _action;
        private bool _isDisposed;

        public DisposeAction(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            if (_isDisposed)
                throw new ObjectDisposedException($"'{nameof(DisposeAction)}' is already disposed!");
            _action.Invoke();
            _isDisposed = true;
        }
    }
}