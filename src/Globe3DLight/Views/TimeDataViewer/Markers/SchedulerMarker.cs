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
using Globe3DLight.Spatial;
using Globe3DLight.ViewModels;

namespace Globe3DLight.Views.TimeDataViewer
{
    public class SchedulerMarker : ViewModelBase
    {
        private int _absolutePositionX;
        private int _absolutePositionY;
        private int _zIndex;
        private Point2D _offset;
        private BaseSchedulerControl _map;
        private bool _first = false;

        internal SchedulerMarker() { }

        public bool IsFreeze { get; set; } = false;

        public Visual Shape { get; set; }

        public void SetLocalPosition(double localPositionX, double localPositionY)
        {
            LocalPosition = new Point2D(localPositionX, localPositionY);

            UpdateAbsolutePosition();
        }

        public Point2D LocalPosition { get; protected set; }
       
        // the map of this marker     
        public BaseSchedulerControl Map
        {
            get
            {
                if (Shape != null && _map == null)
                {
                    IVisual visual = Shape;
                    while (visual != null && !(visual is BaseSchedulerControl))
                    {
                        visual = visual.VisualParent;// VisualTreeHelper.GetParent(visual);
                    }

                    _map = visual as BaseSchedulerControl;
                }

                return _map;
            }
            internal set
            {
                _map = value;
            }
        }

        // offset of marker     
        public virtual Point2D Offset
        {
            get
            {
                return _offset;
            }
            set
            {
               // if (_offset != value)
              //  {
                    _offset = value;
                    UpdateAbsolutePosition();
                //}
            }
        }
      
        // local X position of marker      
        public virtual int AbsolutePositionX
        {
            get => _absolutePositionX;
            set => RaiseAndSetIfChanged(ref _absolutePositionX, value);
        }

        // local Y position of marker        
        public virtual int AbsolutePositionY
        {
            get => _absolutePositionY;            
            set => RaiseAndSetIfChanged(ref _absolutePositionY, value);
        }
       
        // the index of Z, render order     
        public int ZIndex
        {
            get => _zIndex;
            set => RaiseAndSetIfChanged(ref _zIndex, value);
        }

        // calls Dispose on shape if it implements IDisposable, sets shape to null and clears route    
        public virtual void Clear()
        {
            var s = (Shape as IDisposable);
            if (s != null)
            {
                s.Dispose();
                s = null;
            }
            Shape = null;
        }
    
        protected virtual void UpdateAbsolutePosition()
        {
            if (Map != null)
            {
                if (IsFreeze == true && _first == true)
                    return;

                Point2I p = Map.FromLocalToAbsolute(LocalPosition);

                AbsolutePositionX = p.X + (int)Offset.X;
                AbsolutePositionY = p.Y + (int)Offset.Y;

                _first = true;
            }
        }

        public void ResetOffset()
        {
            LocalPosition = Map.FromAbsoluteToLocal(AbsolutePositionX, AbsolutePositionY);

            //  SCSchedulerPoint pos = Map.FromLocalToSchedulerPoint(LocalPositionX, LocalPositionY);

            this._offset = new Point2D(0, 0);
        }

        // forces to update local marker position dot not call it if you don't really need to ;}
        internal void ForceUpdateLocalPosition(BaseSchedulerControl m)
        {
            if (m != null)
            {
                _map = m;
            }
            UpdateAbsolutePosition();
        }
    }
}
