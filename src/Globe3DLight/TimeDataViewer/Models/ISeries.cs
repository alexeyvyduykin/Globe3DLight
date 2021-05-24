using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeDataViewer.Models
{
    public interface ISeries : IMarker
    {
        IList<IInterval> Intervals { get; }

        ISeriesControl? SeriesControl { get; set; }
       
        double MinTime();

        double MaxTime();
    }
}
