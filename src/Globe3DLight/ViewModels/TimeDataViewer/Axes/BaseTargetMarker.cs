#nullable disable
using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Spatial;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    public abstract class BaseTargetMarker
    {
        public Point2D LocalPosition { get; }

        public string Name { get; set; }
    }
}
