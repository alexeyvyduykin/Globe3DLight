#nullable enable
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Diagnostics;
using System;
using Avalonia.Media;
using Avalonia.Visuals;
using Avalonia.Controls;
using Avalonia.VisualTree;
using Avalonia;
using TimeDataViewer.Spatial;
using TimeDataViewer;
using TimeDataViewer.Models;
using TimeDataViewer.Core;

namespace TimeDataViewer.ViewModels
{
    public class MarkerViewModel : ViewModelBase, IMarker
    {
        private int _absolutePositionX;
        private int _absolutePositionY;
        private int _zIndex;

        internal MarkerViewModel() { }

        public Point2D LocalPosition { get; set; }
         
        public virtual int AbsolutePositionX
        {
            get => _absolutePositionX;
            set => RaiseAndSetIfChanged(ref _absolutePositionX, value);
        }
  
        public virtual int AbsolutePositionY
        {
            get => _absolutePositionY;            
            set => RaiseAndSetIfChanged(ref _absolutePositionY, value);
        }
       
        public int ZIndex
        {
            get => _zIndex;
            set => RaiseAndSetIfChanged(ref _zIndex, value);
        }
    }
}
