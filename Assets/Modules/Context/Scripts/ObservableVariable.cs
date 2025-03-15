using System;
using System.Collections.Generic;

namespace Context
{
    public delegate void NewChange<T>(T current);

    public class ObservableVariable<T> : IEquatable<T>
    {
        public event NewChange<T> OnChange;

        private T value;

        public ObservableVariable()
        {
            value = default;
        }

        public ObservableVariable(T defaultValue)
        {
            value = defaultValue;
        }

        public T Get()
        {
            return value;
        }

        public void Set(T newValue)
        {
            Set(newValue, !Equals(newValue));
        }

        public void Set(T newValue, bool notifyEvent)
        {
            value = newValue;

            if (notifyEvent)
            {
                OnChange?.Invoke(value);
            }
        }

        public bool Equals(T other)
        {
            return EqualityComparer<T>.Default.Equals(value, other);
        }

        public static implicit operator T(ObservableVariable<T> observableVariable)
        {
            return observableVariable.value;
        }

        public static implicit operator ObservableVariable<T>(T newValue)
        {
            ObservableVariable<T> observable = new();
            observable.Set(newValue);
            return observable;
        }
    }
}