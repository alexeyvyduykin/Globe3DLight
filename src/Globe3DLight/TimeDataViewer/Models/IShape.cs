using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeDataViewer.Models
{
    public interface IShape
    {
        ISchedulerControl? Scheduler { get; }

        IMarker? Marker { get; }
    }
}
