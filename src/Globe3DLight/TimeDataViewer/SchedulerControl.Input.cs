using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.Metadata;
using Avalonia.Input.GestureRecognizers;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Visuals;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Metadata;
using Avalonia.Styling;
using Avalonia.VisualTree;
using TimeDataViewer.ViewModels;
using TimeDataViewer.Core;
using TimeDataViewer.Spatial;
using System.Xml;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Input.TextInput;

namespace TimeDataViewer
{
    public delegate void MousePositionChangedEventHandler(Point2D point);

    public partial class SchedulerControl
    {
        private Cursor? _cursorBefore;
        private int _onMouseUpTimestamp = 0;
        private Point2D _mousePosition = new();
        private bool _disableAltForSelection = false;
        private bool _isDragging = false;
        private Point2D _mouseDown;

        public bool IgnoreMarkerOnMouseWheel { get; set; } = true;

        public Point2D MousePosition
        {
            get => _mousePosition;            
            protected set
            {
                _mousePosition = value;
                OnMousePositionChanged?.Invoke(_mousePosition);
                //Debug.WriteLine($"SchedulerControl -> OnMousePositionChanged -> Count = {OnMousePositionChanged?.GetInvocationList().Length}");
            }
        }

        private void SchedulerControl_PointerWheelChanged(object? sender, PointerWheelEventArgs e)
        {
            if (IgnoreMarkerOnMouseWheel == true && _area.IsDragging == false)
            {
                Zoom = (e.Delta.Y > 0) ? ((int)Zoom) + 1 : ((int)(Zoom + 0.99)) - 1;
                
                //var ps = (this as Visual).PointToScreen(new Point(_area.ZoomScreenPosition.X, _area.ZoomScreenPosition.Y));

                //Stuff.SetCursorPos((int)ps.X, (int)ps.Y);
            }
        }

        private void SchedulerControl_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.PointerUpdateKind == PointerUpdateKind.RightButtonPressed)
            {
                var p = e.GetPosition(this);

                _mouseDown = new Point2D(p.X, p.Y);
            }
        }

        private void SchedulerControl_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            if (_area.IsDragging == true)
            {
                if (_isDragging == true)
                {
                    _onMouseUpTimestamp = (int)e.Timestamp & int.MaxValue;
                    _isDragging = false; 
                    
                    if(_cursorBefore == null)
                    {
                        _cursorBefore = new(StandardCursorType.Arrow);
                    }

                    Cursor = _cursorBefore;               
                    e.Pointer.Capture(null);
                }
                _area.EndDrag();
                _mouseDown = Point2D.Empty;
            }
            else
            {
                if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed == true)
                {
                    _mouseDown = Point2D.Empty;
                }
                                    
                InvalidateVisual();                
            }
        }

        private void SchedulerControl_PointerMoved(object? sender, PointerEventArgs e)
        {
            var MouseScreenPosition = e.GetPosition(this);
                
            _area.ZoomScreenPosition = new Point2I((int)MouseScreenPosition.X, (int)MouseScreenPosition.Y);

            MousePosition = _area.FromScreenToLocal((int)MouseScreenPosition.X, (int)MouseScreenPosition.Y);

            // wpf generates to many events if mouse is over some visual and OnMouseUp is fired, wtf, anyway...         
            if (((int)e.Timestamp & int.MaxValue) - _onMouseUpTimestamp < 55)
            {              
                return;
            }

            if (_area.IsDragging == false && _mouseDown.IsEmpty == false)
            {
                // cursor has moved beyond drag tolerance
                if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed == true)
                {
                    if (Math.Abs(MouseScreenPosition.X - _mouseDown.X) * 2 >= 2 ||
                        Math.Abs(MouseScreenPosition.Y - _mouseDown.Y) * 2 >= 2)
                    {
                        _area.BeginDrag(_mouseDown);
                    }
                }
            }

            if (_area.IsDragging == true)
            {
                if (_isDragging == false)
                {
                    _isDragging = true;           
                    _cursorBefore = Cursor;
                    Cursor = new Cursor(StandardCursorType.SizeWestEast);
                    e.Pointer.Capture(this);       
                }

                var mouseCurrent = new Point2D(MouseScreenPosition.X, MouseScreenPosition.Y);
               
                _area.Drag(mouseCurrent);

                UpdateMarkersOffset();

                InvalidateVisual();
            }
        }
    }
}
