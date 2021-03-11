using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.Data
{
    public interface IEventState
    {
        IEventState FromHit(IEventState state0, IEventState state1, double t);

        double t { get; }
    }
}
