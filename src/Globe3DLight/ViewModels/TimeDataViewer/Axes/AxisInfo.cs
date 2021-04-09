using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    public struct AxisInfo
    {
        public List<AxisLabelPosition> Labels { get; set; }

        public List<AxisLabelPosition> FollowLabels { get; set; }

        public string MinLabel { get; set; }

        public string MaxLabel { get; set; }

        public AxisLabelPosition DynamicLabel { get; set; }

        public bool IsDynamicLabelEnable { get; set; }

        public bool IsFollowLabelsMode { get; set; }

        public EAxisCoordType CoordType { get; set; }

        public double MinValue { get; set; }

        public double MaxValue { get; set; }
    }
}
