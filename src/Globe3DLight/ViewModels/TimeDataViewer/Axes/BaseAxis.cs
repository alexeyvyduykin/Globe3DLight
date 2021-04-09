using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Spatial;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    public enum EAxisCoordType
    {
        X,
        Y
    }
    public struct AxisLabelPosition
    {
        public string Label { get; set; }
        public double Value { get; set; }
    }

    public abstract class BaseAxis
    {
        private bool _isInversed = false;
        private int _length;
        private string _header = "Default Header";

        public void UpdateAreaSize(int width, int height)
        {
            switch (CoordType)
            {
                case EAxisCoordType.X:
                    _length = width;
                    break;
                case EAxisCoordType.Y:
                    _length = height;
                    break;
                default:
                    _length = 0;
                    break;
            }

            //switch (Type)
            //{
            //    case AxisPosition.Bottom:
            //    case AxisPosition.Top:
            //        _length = width;
            //        break;
            //    case AxisPosition.Left:
            //    case AxisPosition.Right:
            //        _length = height; 
            //        break;
            //    default:
            //        _length = 0; 
            //        break;
            //}

            if (OnAxisChanged != null)
            {
                OnAxisChanged();
            }
        }
       
        public bool IsInversed
        {
            get => _isInversed;            
            set => _isInversed = value;            
        }

        public bool IsDynamicLabelEnable { get; set; } = false;

        //public Transform Transform
        //{
        //    get
        //    {
        //        switch (CoordType)
        //        {
        //            case EAxisCoordType.X:
        //                return new MatrixTransform(1.0, 0.0, 0.0, 1.0, 0.0, 0.0);
        //            case EAxisCoordType.Y:
        //                return new MatrixTransform(0.0, -1.0, -1.0, 0.0, 0.0, 0.0);
        //            default:
        //                return Transform.Identity;
        //        }


        //        //switch (Type)
        //        //{
        //        //    case AxisPosition.Bottom:
        //        //        return new MatrixTransform(1.0, 0.0, 0.0, 1.0, 0.0, 0.0);
        //        //    case AxisPosition.Left:
        //        //        return new MatrixTransform(0.0, -1.0, -1.0, 0.0, 0.0, 0.0);
        //        //    case AxisPosition.Top:
        //        //        return new MatrixTransform(1.0, 0.0, 0.0, -1.0, 0.0, -Length);
        //        //    case AxisPosition.Right:
        //        //        return new MatrixTransform(0.0, -1.0, 1.0, 0.0, Length, 0.0);
        //        //    default:
        //        //        return Transform.Identity;
        //        //}
        //    }
        //}
      
        public int Length => _length;

        public void UpdateAxis()
        {
            if (OnAxisChanged != null)
            {
                OnAxisChanged();
            }
        }

        public string Header 
        {
            get => _header; 
            set => _header = value;
        }

        public EAxisCoordType CoordType { get; set; }

        public event AxisChanged OnAxisChanged;
        //     public event SCAxisLengthChanged OnLengthChanged;

        protected static double Clip(double n, double minValue, double maxValue)
        {
            return Math.Min(Math.Max(n, minValue), maxValue);
        }

        protected static int Clip(int n, int minValue, int maxValue)
        {
            return Math.Min(Math.Max(n, minValue), maxValue);
        }

        public abstract double FromAbsoluteToLocal(int pixel);

        public abstract int FromLocalToAbsolute(double value);

        public abstract AxisInfo AxisInfo { get; }

        public abstract void UpdateWindow(RectI window);

        public abstract void UpdateViewport(RectD viewport);

        public abstract void UpdateScreen(RectD screen);

        public abstract void UpdateFollowLabelPosition(ITargetMarker marker);
    }
}
