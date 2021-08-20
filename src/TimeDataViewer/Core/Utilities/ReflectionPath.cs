using System;
using System.Reflection;

namespace TimeDataViewer.Core
{
    /// <summary>
    /// Provides functionality to reflect a path of properties.
    /// </summary>
    public class ReflectionPath
    {
        private readonly string[] _items;
        private readonly PropertyInfo[] _infos;
        private readonly Type[] _reflectedTypes;

        public ReflectionPath(string path)
        {
            _items = path != null ? path.Split('.') : new string[0];
            _infos = new PropertyInfo[_items.Length];
            _reflectedTypes = new Type[_items.Length];
        }

        public object? GetValue(object instance)
        {
            if (TryGetValue(instance, out object? result))
            {
                return result;
            }

            throw new InvalidOperationException("Could not find property " + string.Join(".", _items) + " in " + instance);
        }

        public bool TryGetValue(object instance, out object? result)
        {
            var current = instance;
            for (int i = 0; i < _items.Length; i++)
            {
                if (current == null)
                {
                    result = null;
                    return true;
                }

                var currentType = current.GetType();

                var pi = _infos[i];
                if (pi == null || _reflectedTypes[i] != currentType)
                {
                    pi = _infos[i] = currentType.GetRuntimeProperty(_items[i]);
                    _reflectedTypes[i] = currentType;
                }

                if (pi == null)
                {
                    result = null;
                    return false;
                }

                current = pi.GetValue(current, null);
            }

            result = current;
            return true;
        }
    }
}
