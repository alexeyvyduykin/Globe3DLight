#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Spatial;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    public interface ISCTargetMarker
    {
        Point2D LocalPosition { get; }
        string Name { get; set; }
    }
}
