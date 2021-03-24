using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Models.Renderer
{
    public interface ICache<TKey, TValue>
    {
        /// <summary>
        /// Gets value from storage.
        /// </summary>
        /// <param name="key">The key object.</param>
        /// <returns>The value from storage.</returns>
        TValue Get(TKey key);

        /// <summary>
        /// Sets or adds new value to storage.
        /// </summary>
        /// <param name="key">The key object.</param>
        /// <param name="value">The value object.</param>
        void Set(TKey key, TValue value);

        /// <summary>
        /// Resets cache storage.
        /// </summary>
        void Reset();
    }
}
