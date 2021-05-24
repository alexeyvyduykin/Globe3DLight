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
using TimeDataViewer.Models;

namespace TimeDataViewer
{
    public class Factory
    {
        public ISeries CreateSeries(string category, ISeriesControl series)
        {
            return new SeriesViewModel() 
            {
                Name = category,
                ZIndex = 30,            
                SeriesControl = series
            };
        }

        public IInterval CreateInterval(double left, double right, ISeriesControl series)
        {
            return new IntervalViewModel(left, right) 
            {
                ZIndex = 100,            
                SeriesControl = series, 
            };
        }
    }
}
