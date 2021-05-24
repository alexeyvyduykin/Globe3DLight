using Avalonia.Controls;
using TimeDataViewer.Models;
using Avalonia.Media;
using Avalonia;

namespace TimeDataViewer.Shapes
{
    public abstract class BaseIntervalVisual : BaseVisual
    {
        //public static readonly StyledProperty<Series> SeriesProperty =    
        //    AvaloniaProperty.Register<IntervalVisual, Series>(nameof(Series));

        //public Series Series
        //{
        //    get { return GetValue(SeriesProperty); }
        //    set { SetValue(SeriesProperty, value); }
        //}

        public abstract BaseIntervalVisual Clone(IInterval interval);
    }
}
