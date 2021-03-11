using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.Data
{
    public interface IEventList<T> where T : IEventInterval
    {
        void Add(T interval);

        void Clear();

        T ActiveInterval(double t);
    }
}
