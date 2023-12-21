using System;
using System.Collections.Generic;

namespace FoodlesUtilities.Observables
{
    [Serializable]
    public class ObservableList<T> : List<T>
    {
        // Define the event that will be triggered when a value is added or changed
        public event Action CollectionChanged; 
        public event Action<T> ValueAdded;
        public event Action<T> ValueRemoved;
        public event Action<T, T> ValueChanged;

        // Override the Add method to trigger the event when a value is added
        public new void Add(T item)
        {
            base.Add(item);
            OnValueAdded(item);
        }

        // Method to trigger the events when a value is added
        protected virtual void OnValueAdded(T item)
        {
            ValueAdded?.Invoke(item);
            
            CollectionChanged?.Invoke();
        }
        
        
        // Override the Remove method to trigger the event when a value is removed
        public new void Remove(T item)
        {
            base.Remove(item);
            
            OnValueRemoved(item);
        }
        
        // Method to trigger the events when a value is removed
        protected virtual void OnValueRemoved(T item)
        {
            ValueRemoved?.Invoke(item);
            
            CollectionChanged?.Invoke();
        }

        // Override the indexer to trigger the event when a value is changed
        public new T this[int index]
        {
            get => base[index];
            set
            {
                var oldValue = base[index];
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
