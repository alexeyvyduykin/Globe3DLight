﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Renderer
{
    public class Cache<TKey, TValue> : ICache<TKey, TValue>
    {
        private IDictionary<TKey, TValue> _storage;
        private readonly Action<TValue> _dispose;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cache{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="dispose">The dispose action.</param>
        public Cache(Action<TValue> dispose = null)
        {
            _dispose = dispose;
            _storage = new Dictionary<TKey, TValue>();
        }

        /// <inheritdoc/>
        public TValue Get(TKey key)
        {
            if (_storage.TryGetValue(key, out var data))
            {
                return data;
            }
            return default;
        }

        /// <inheritdoc/>
        public void Set(TKey key, TValue value)
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

        /// <inheritdoc/>
        public void Reset()
        {
            if (_storage != null)
            {
                if (_dispose != null)
                {
                    foreach (var data in _storage)
                    {
                        _dispose(data.Value);
                    }
                }
                _storage.Clear();
            }
            _storage = new Dictionary<TKey, TValue>();
        }
    }
}
