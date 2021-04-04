#nullable enable
using System;
using System.Collections.Generic;
using Globe3DLight.Models.Renderer;

namespace Globe3DLight.ViewModels.Renderer
{
    public class Cache<TKey, TValue> : ICache<TKey, TValue> where TKey: notnull
    {
        private IDictionary<TKey, TValue?> _storage;
        private readonly Action<TValue>? _dispose;

        public Cache(Action<TValue>? dispose = null)
        {
            _dispose = dispose;
            _storage = new Dictionary<TKey, TValue?>();
        }

        public TValue? Get(TKey key)
        {
            return (_storage.TryGetValue(key, out var data)) ? data : default;
        }

        public void Set(TKey key, TValue? value)
        {
            if (_storage.ContainsKey(key))
            {
                _storage[key] = value;
            }
            else
            {
                _storage.Add(key, value);
            }
        }

        public void Reset()
        {
            if (_dispose != null)
            {
                foreach (var data in _storage)
                {
                    if (data.Value is not null)
                    {
                        _dispose(data.Value);
                    }
                }
            }

            _storage.Clear();
            _storage = new Dictionary<TKey, TValue?>();
        }
    }
}
