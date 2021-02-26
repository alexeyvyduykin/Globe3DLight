using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Timer
{
    public interface IAcceleratedTimer : ITimer
    {
        void Faster();

        void Slower();
    }
}
