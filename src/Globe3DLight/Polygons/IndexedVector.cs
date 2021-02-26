using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Globe3DLight
{
    internal struct IndexedVector<T> : IEquatable<IndexedVector<T>>
    {
        public IndexedVector(T vector, int index)
        {
            _vector = vector;
            _index = index;
        }

        public T Vector
        {
            get { return _vector; }
        }

        public int Index
        {
            get { return _index; }
        }

        public static bool operator ==(IndexedVector<T> left, IndexedVector<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IndexedVector<T> left, IndexedVector<T> right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}, {1}", _vector, _index);
        }

        public override int GetHashCode()
        {
            return _vector.GetHashCode() ^ _index.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is IndexedVector<T>))
                return false;

            return this.Equals((IndexedVector<T>)obj);
        }

        #region IEquatable<IndexedVector<T>> Members

        public bool Equals(IndexedVector<T> other)
        {
            return (_vector.Equals(other._vector)) && (_index == other._index);
        }

        #endregion

        private T _vector;
        private int _index;
    }

}
