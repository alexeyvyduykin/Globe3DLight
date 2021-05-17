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
using Globe3DLight.ViewModels;

namespace TimeDataViewer.ViewModels
{
    public class MarkerViewModel : ViewModelBase
    {
        private int _absolutePositionX;
        private int _absolutePositionY;
        private int _zIndex;
        private IScheduler? _scheduler;
        private bool _first = false;

        internal MarkerViewModel() { }

        public bool IsFreeze { get; set; } = false;

        public Visual Shape { get; set; }

        public void SetLocalPosition(double localPositionX, double localPositionY)
        {
            LocalPosition = new Point2D(localPositionX, localPositionY);

            UpdateAbsolutePosition();
        }

        public Point2D LocalPosition { get; protected set; }
         
        public IScheduler? Scheduler
        {
            get
            {
                if (Shape is not null && _scheduler is null)
                {
                    IVisual visual = Shape;
                    while (visual != null && !(visual is IScheduler))
                    {
                        visual = visual.VisualParent;// VisualTreeHelper.GetParent(visual);
                    }

                    _scheduler = visual as IScheduler;
                }

                return _scheduler;
            }
            internal set
            {
                _scheduler = value;
            }
        }

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

        protected virtual void UpdateAbsolutePosition()
        {
            if (Scheduler is not null)
            {
                if (IsFreeze == true && _first == true)
                    return;

                var p = Scheduler.FromLocalToAbsolute(LocalPosition);

                AbsolutePositionX = p.X;
                AbsolutePositionY = p.Y;

                _first = true;
            }
        }

        internal void ForceUpdateLocalPosition(IScheduler sch)
        {
            if (sch is not null)
            {
                Scheduler = sch;
            }

            UpdateAbsolutePosition();
        }
    }
}
