using System;
#if UNITY_64
using UnityEngine;
using UnityEngine.Events;
#endif

// ReSharper disable CheckNamespace
// ReSharper disable Unity.PerformanceCriticalCodeInvocation
namespace FoodlesUtilities.Observables
{
    [Serializable]
    public class ObservableVariable<T>
    {
        public event Action<T> OnValueChanged;

#if UNITY_64
        [SerializeField] private T _value;
        [SerializeField] private UnityEvent<T> onValueChangedUnity;
#else
        private T _value;
#endif

        public ObservableVariable(T initialValue = default)
        {
            _value = initialValue;

            OnValueChanged = null;
#if UNITY_64
            onValueChangedUnity = new UnityEvent<T>();
#endif
        }

        public T Value
        {
            get => _value;
            set
            {
                _value = value;

                OnValueChanged?.Invoke(_value);
#if UNITY_64
                onValueChangedUnity?.Invoke(_value);
#endif
            }
        }
    }
}
