using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeDataViewer.Models
{
    public interface IInterval : IMarker
    {
        double Left { get; }

        double Right { get; }
        
        ISeries? Series { get; set; }

        ISeriesControl? SeriesControl { get; set; }
    }
}
