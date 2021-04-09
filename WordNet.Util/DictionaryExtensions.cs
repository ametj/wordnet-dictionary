using System;
using System.Collections.Generic;

namespace WordNet.Util
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> factoryMethod)
        {
            return dict.TryGetValue(key, out TValue value) ? value : dict[key] = factoryMethod(key);
        }
    }
}
