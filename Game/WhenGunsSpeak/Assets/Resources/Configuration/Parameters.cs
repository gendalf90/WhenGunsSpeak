using System.Collections.Generic;
using UnityEngine;

namespace Configuration
{
    class Parameters : MonoBehaviour
    {
        private static readonly Dictionary<string, object> storage = new Dictionary<string, object>();

        public T GetLocalOrDefault<T>(string name)
        {
            object value;

            if(storage.TryGetValue(name, out value))
            {
                return (T)value;
            }

            return default(T);
        }

        public void SetLocal<T>(string name, T value)
        {
            storage[name] = value;
        }

        public void ClearLocal(string name)
        {
            storage.Remove(name);
        }
    }
}
