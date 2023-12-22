using System;
using System.Collections.Generic;

namespace FoodlesUtilities.Observables
{
    [Serializable]
    public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        // Define the event that will be triggered when the collection is changed
        public event Action CollectionChanged;
        public event Action<TKey, TValue> ValueAdded;
        public event Action<TKey, TValue> ValueRemoved;
        public event Action<TKey, TValue, TValue> ValueChanged;

        // Override the Add method to trigger the event when a key-value pair is added
        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            OnValueAdded(key, value);
        }

        // Method to trigger the events when a key-value pair is added
        protected virtual void OnValueAdded(TKey key, TValue value)
        {
            ValueAdded?.Invoke(key, value);
            CollectionChanged?.Invoke();
        }

        // Override the Remove method to trigger the event when a key-value pair is removed
        public new bool Remove(TKey key)
        {
            if (base.TryGetValue(key, out var value))
            {
                base.Remove(key);
                OnValueRemoved(key, value);
                return true;
            }
            return false;
        }

        // Method to trigger the events when a key-value pair is removed
        protected virtual void OnValueRemoved(TKey key, TValue value)
        {
            ValueRemoved?.Invoke(key, value);
            CollectionChanged?.Invoke();
        }

        // Override the indexer to trigger the event when a value is changed
        public new TValue this[TKey key]
        {
            get => base[key];
            set
            {
                if (TryGetValue(key, out var oldValue))
                {
                    base[key] = value;
                    OnValueChanged(key, oldValue, value);
                }
            }
        }

        // Method to trigger the event when a value is changed
        protected virtual void OnValueChanged(TKey key, TValue oldValue, TValue newValue)
        {
            ValueChanged?.Invoke(key, oldValue, newValue);
            CollectionChanged?.Invoke();
        }
    }
}