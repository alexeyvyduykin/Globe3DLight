using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeDataViewer.Core
{
    public record AxisInfo
    {
        public IList<AxisLabelPosition>? Labels { get; init; }

        public string? MinLabel { get; init; }

        public string? MaxLabel { get; init; }

        public AxisLabelPosition? DynamicLabel { get; init; }

        public AxisType Type { get; init; }

        public double MinValue { get; init; }

        public double MaxValue { get; init; }
    }
}
