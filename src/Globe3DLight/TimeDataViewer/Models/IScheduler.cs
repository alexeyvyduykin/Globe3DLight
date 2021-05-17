using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeDataViewer.Spatial;
using Avalonia.Controls;

namespace TimeDataViewer.Models
{
    public interface IScheduler : IControl
    {
        Point2D FromAbsoluteToLocal(int x, int y);

        Point2I FromLocalToAbsolute(Point2D point);

        void ShowTooltip(Control placementTarget, Control tooltip);

        void HideTooltip();

        RectI AbsoluteWindow { get; }

        DateTime Epoch { get; }

        event EventHandler OnZoomChanged;

        event EventHandler OnSizeChanged;
    }
}
