using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.Data
{
    public interface IEventList<T> where T : IEventState
    {
        T ActiveState { get; }

        bool HasActiveState { get; }

        void Add(T from, T to);

        void Clear();

        void Update(double t);
    }
}
