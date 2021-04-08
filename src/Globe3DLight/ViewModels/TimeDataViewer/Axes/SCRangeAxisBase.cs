#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Spatial;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    public abstract class SCRangeAxisBase : SCAxisBase
    {
        public abstract void UpdateDynamicLabelPosition(Point2 point);
    }
}
