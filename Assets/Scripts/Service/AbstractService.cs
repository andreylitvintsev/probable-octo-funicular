using UnityEngine;

namespace Service
{
    public abstract class AbstractService : MonoBehaviour
    {
        public void Initialize()
        {
            OnInitialize();
        }

        private void OnDestroy()
        {
            OnDeinitialize();
        }

        protected abstract void OnInitialize(); 
        protected abstract void OnDeinitialize();
    }
}