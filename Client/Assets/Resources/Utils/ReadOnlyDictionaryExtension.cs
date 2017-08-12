using System.Collections;
using System.Collections.Generic;

public interface IReadOnlyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
{
    IEnumerable<TKey> Keys { get; }
    IEnumerable<TValue> Values { get; }
    TValue this[TKey key] { get; }
    int Count { get; }
    bool TryGetValue(TKey key, out TValue value);
}

public static class ReadOnlyDictionaryExtension
{
    class ReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> internalDictionary;

        public ReadOnlyDictionary(Dictionary<TKey, TValue> baseDictionary)
        {
            internalDictionary = baseDictionary;
        }

        public TValue this[TKey key]
        {
            get
            {
                return internalDictionary[key];
            }
        }

        public int Count
        {
            get
            {
                return internalDictionary.Count;
            }
        }

        public IEnumerable<TKey> Keys
        {
            get
            {
                return internalDictionary.Keys;
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                return internalDictionary.Values;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return internalDictionary.GetEnumerator();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return internalDictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return internalDictionary.GetEnumerator();
        }
    }

    public static IReadOnlyDictionary<TKey, TValue> AsReadOnlyDictionary<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
    {
        return new ReadOnlyDictionary<TKey, TValue>(dictionary);
    }
}


