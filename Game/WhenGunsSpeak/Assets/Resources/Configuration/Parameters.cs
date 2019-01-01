using System;
using System.Collections.Generic;
using UnityEngine;

namespace Configuration
{
    class Parameters : MonoBehaviour
    {
        private static readonly Dictionary<string, object> storage = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public T Get<T>(string name)
        {
            return (T)storage[name];
        }

        public void Set<T>(string name, T value)
        {
            storage[name] = value;
        }

        public void Remove(string name)
        {
            storage.Remove(name);
        }

        public void Clear()
        {
            storage.Clear();
        }
    }
}
