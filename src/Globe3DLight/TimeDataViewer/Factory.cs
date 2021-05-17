using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Metadata;
using Avalonia.Styling;
using Avalonia.VisualTree;
using TimeDataViewer.ViewModels;
using TimeDataViewer;
using TimeDataViewer.Spatial;
using System.Xml;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Controls.Metadata;
using Avalonia.Input.GestureRecognizers;
using Avalonia.Input.TextInput;
using Avalonia.Interactivity;
using TimeDataViewer.Shapes;

namespace TimeDataViewer
{
    public class Factory
    {
        public SeriesViewModel CreateSeries(string category)
        {
            var marker = new SeriesViewModel(category);

            marker.Shape = new StringVisual(marker);

            return marker;
        }

        public IntervalViewModel CreateInterval(Interval ival, SeriesViewModel parent, BaseIntervalVisual template)
        {
            var marker = new IntervalViewModel(ival.Left, ival.Right);
            
            marker.Series = parent;

            var visual = template.Clone();

            visual.DataContext = marker;

            marker.Shape = visual;

            parent.Intervals.Add(marker);

            return marker;
        }        
    }
}
