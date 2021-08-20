using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Input;
using Avalonia.Layout;
using TimeDataViewer.Spatial;

namespace TimeDataViewer
{
    public static class Extensions
    {
        public static bool IsEmpty(this OxyRect rect)
        {
            return rect.Left == 0.0 && rect.Right == 0.0 && rect.Width == 0.0 && rect.Height == 0.0;
        }

        public static bool IsEmpty(this ScreenPoint point)
        {
            return point.X == 0.0 && point.Y == 0.0;
        }

        public static void AddRange<T>(this ObservableCollection<T> arr, IEnumerable<T> values)
        {
            foreach (var item in values)
            {
                arr.Add(item);
            }
        }

        public static Core.OxyMouseWheelEventArgs ToMouseWheelEventArgs(this PointerWheelEventArgs e, IInputElement relativeTo)
        {
            return new Core.OxyMouseWheelEventArgs
            {
                Position = e.GetPosition(relativeTo).ToScreenPoint(),
                Delta = (int)(e.Delta.Y + e.Delta.X) * 120
            };
        }

        public static ScreenPoint ToScreenPoint(this Point pt)
        {
            return new ScreenPoint(pt.X, pt.Y);
        }

        public static Core.OxyMouseDownEventArgs ToMouseDownEventArgs(this PointerPressedEventArgs e, IInputElement relativeTo)
        {
            var point = e.GetCurrentPoint(relativeTo);

            return new Core.OxyMouseDownEventArgs
            {
                ChangedButton = point.Properties.PointerUpdateKind.Convert(),
                Position = e.GetPosition(relativeTo).ToScreenPoint(),
            };
        }
        public static Core.OxyMouseEventArgs ToMouseReleasedEventArgs(this PointerReleasedEventArgs e, IInputElement relativeTo)
        {
            return new Core.OxyMouseEventArgs
            {
                Position = e.GetPosition(relativeTo).ToScreenPoint(),
            };
        }

        public static Core.OxyMouseEventArgs ToMouseEventArgs(this PointerEventArgs e, IInputElement relativeTo)
        {
            return new Core.OxyMouseEventArgs
            {
                Position = e.GetPosition(relativeTo).ToScreenPoint(),
            };
        }

        public static Core.OxyMouseButton Convert(this PointerUpdateKind pointerUpdateKind)
        {
            return pointerUpdateKind switch
            {
                PointerUpdateKind.LeftButtonPressed => Core.OxyMouseButton.Left,
                PointerUpdateKind.MiddleButtonPressed => Core.OxyMouseButton.Middle,
                PointerUpdateKind.RightButtonPressed => Core.OxyMouseButton.Right,
                _ => Core.OxyMouseButton.None,
            };
        }

        public static HorizontalAlignment ToAvalonia(this Core.HorizontalAlignment horizontal)
        {
            return horizontal switch
            {
                Core.HorizontalAlignment.Left => HorizontalAlignment.Left,
                Core.HorizontalAlignment.Center => HorizontalAlignment.Center,
                Core.HorizontalAlignment.Right => HorizontalAlignment.Right,
                _ => HorizontalAlignment.Stretch,
            };
        }

        public static VerticalAlignment ToAvalonia(this Core.VerticalAlignment vertical)
        {
            return vertical switch
            {
                Core.VerticalAlignment.Bottom => VerticalAlignment.Bottom,
                Core.VerticalAlignment.Middle => VerticalAlignment.Center,
                Core.VerticalAlignment.Top => VerticalAlignment.Top,
                _ => VerticalAlignment.Stretch,
            };
        }
    }
}
