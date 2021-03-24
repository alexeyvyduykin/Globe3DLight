using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.Models.Data
{
    public interface IEventInterval
    {
        double BeginTime { get; }
        double EndTime { get; }
        bool IsRange(double t);
        bool IsForward(double t);
        bool IsBackward(double t);
    }
}
