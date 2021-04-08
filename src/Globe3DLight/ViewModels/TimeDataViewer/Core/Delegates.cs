#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Spatial;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    public delegate void SCDragChanged();
    public delegate void SCZoomChanged();
    public delegate void SCSizeChanged(int width, int height);

    public delegate void SCTargetMarkerPositionChanged(ISCTargetMarker marker);
    public delegate void SchedulerTypeChanged(SCTimeSchedulerProvider type);
    public delegate void SCPositionChanged(Point2 point);

    public delegate void SCAxisLengthChanged(int length);
    public delegate void AxisChanged();
    public delegate void AxisRect3Changed(int w, int h);


    public delegate void SchedulerWindowChanged(SCWindow window);
    public delegate void SchedulerViewportChanged(SCViewport viewport);

    public delegate void AxisShapeChanged(SCAxisInfo info);

}
