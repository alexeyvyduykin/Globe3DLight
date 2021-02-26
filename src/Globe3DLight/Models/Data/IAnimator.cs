using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Data
{
    public interface IAnimator : IObservableObject
    {
        void Animate(double t);
    }
}
