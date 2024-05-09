using System;
using UnityEngine;

namespace View
{
    public abstract class AbstractView<TData> : MonoBehaviour
    {
        private TData _data = default;
        public TData Data => _data;

        public event Action OnDispose = null;

        public void Initialize(TData data)
        {
            _data = data;
            OnInitialize(data);
        }

        private void OnDestroy()
        {
            OnDispose?.Invoke();
            OnDispose = null;
            
            OnDeinitialize();
        }

        protected abstract void OnInitialize(TData data);
        
        protected abstract void OnDeinitialize();
    }
}