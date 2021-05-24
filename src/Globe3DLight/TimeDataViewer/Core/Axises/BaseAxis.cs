using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeDataViewer.Spatial;
using TimeDataViewer.ViewModels;
using System.Diagnostics;

namespace TimeDataViewer.Core
{
    public enum AxisType
    {
        X,
        Y
    }

    public record AxisLabelPosition
    {
        public string? Label { get; init; }
        public double Value { get; init; }
    }

    public abstract class BaseAxis : IAxis
    {
        public event EventHandler? OnAxisChanged;

        public bool HasInversion { get; set; }

        public bool IsDynamicLabelEnable { get; set; }

        public string Header { get; set; } = "Header";

        public AxisType Type { get; set; }

        public double MinValue { get; protected set; }

        public double MaxValue { get; protected set; }

        public double MinClientValue { get; protected set; }

        public double MaxClientValue { get; protected set; }

        public int MinPixel { get; protected set; }

        public int MaxPixel { get; protected set; }

        public abstract AxisInfo AxisInfo { get; }
                         
        public abstract double FromAbsoluteToLocal(int pixel);

        public abstract int FromLocalToAbsolute(double value);
      
        protected virtual void Invalidate()
        {
            OnAxisChanged?.Invoke(this, EventArgs.Empty);
            //Debug.WriteLine($"BaseAxis -> OnAxisChanged -> Count = {OnAxisChanged?.GetInvocationList().Length}");
        }

        public abstract void UpdateWindow(RectI window);

        public abstract void UpdateViewport(RectD viewport);

        public abstract void UpdateClientViewport(RectD clientViewport);
    }
}
