using System;
using System.Collections.Generic;
using UnityEngine;

namespace Service
{
    public class ServiceProvider : MonoBehaviour
    {
        [SerializeField] private List<AbstractService> _services = null;
        
        private readonly Dictionary<Type, AbstractService> _servicesLookup = new();

        public void Initialize()
        {
            foreach (var service in _services) {
                DontDestroyOnLoad(service);
                _servicesLookup.Add(service.GetType(), service);
                service.Initialize();
            }
        }

        public TService GetService<TService>() where TService : AbstractService
        {
            return (TService) _servicesLookup[typeof(TService)];
        }
    }
}