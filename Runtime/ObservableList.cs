using System;
using System.Collections.Generic;
/*#if UNITY_64
using UnityEngine;
using UnityEngine.Events;
#endif*/

namespace FoodlesUtilities
{
    [Serializable]
    public class ObservableList<T> : List<T>
    {
        // Define the event that will be triggered when a value is added or changed
        public event Action CollectionChanged; 
        public event Action<T> ValueAdded;
        public event Action<T, T> ValueChanged;
        
/*#if UNITY_64
        [SerializeField] private UnityEvent onCollectionChangedUnity = new UnityEvent();
        [SerializeField] private UnityEvent<T, T> onValueChangedUnity = new UnityEvent<T, T>();
        [SerializeField] private UnityEvent<T> onValueAddedUnity = new UnityEvent<T>();
#endif

        public ObservableList()
        {
#if UNITY_64
            CollectionChanged += () => onCollectionChangedUnity?.Invoke();
            ValueChanged += (oldValue, newValue) => onValueChangedUnity?.Invoke(oldValue, newValue);
            ValueAdded += newValue => onValueAddedUnity?.Invoke(newValue);
#endif
        }*/

        // Override the Add method to trigger the event when a value is added
        public new void Add(T item)
        {
            base.Add(item);
            OnValueAdded(item);
        }

        // Method to trigger the event when a value is added
        protected virtual void OnValueAdded(T item)
        {
            ValueAdded?.Invoke(item);
            
            CollectionChanged?.Invoke();
        }

        // Override the indexer to trigger the event when a value is changed
        public new T this[int index]
        {
            get => base[index];
            set
            {
                T oldValue = base[index];
                base[index] = value;
                OnValueChanged(oldValue, value);
            }
        }

        // Method to trigger the event when a value is changed
        protected virtual void OnValueChanged(T oldValue, T newValue)
        {
            ValueChanged?.Invoke(oldValue, newValue);
            
            CollectionChanged?.Invoke();
        }
    }
}
