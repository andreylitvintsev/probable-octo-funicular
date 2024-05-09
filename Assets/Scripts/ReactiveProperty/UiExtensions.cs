using System;
using UnityEngine.Events;
using UnityEngine.UI;
using View;

namespace ReactiveProperty
{
    public static class UiExtensions
    {
        public static IDisposable SubscribeOnClick(this Button button, UnityAction onEvent)
        {
            button.onClick.AddListener(onEvent);
            
            return new DisposeAction(() => 
                button.onClick.RemoveListener(onEvent)
            );
        }

        public static void DisposeWith<T>(this IDisposable disposable, AbstractView<T> abstractView)
        {
            abstractView.OnDispose += disposable.Dispose;
        }
    }
}